using FinanceSystem.Domain.FinanceAccounts.Exceptions;
using FinanceSystem.Domain.Contract.Payments;
using FinanceSystem.Domain.PaymentServiceProviders;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Exceptions;
using FinanceSystem.Domain.Payments.Enums;
using FinanceSystem.Domain.Payments.Exceptions;
using FinanceSystem.Domain.Users;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.Payments;

public sealed class Payment : EntityBase<long>, IAggregateRoot
{

    public IEventPublisher Publisher { get; set; }

    public Money Amount { get; private set; }
    public PaymentPurpose Purpose { get; private set; }
    public PaymentChannel Channel { get; private set; }
    public PaymentState State { get; private set; }

    public long UserId { get; private set; }
    public PaymentDirection Direction { get; private set; }
    public string ServiceId { get; private set; }
    public string ReferenceId { get; private set; }
    public long? BillId { get; private set; }
    public PspCode? PspCode { get; private set; }
    public string? Token { get; private set; }
    public string? RRN { get; private set; }
    public Money? RedirectedAmount { get; private set; }
    public string? ReferenceNumber { get; private set; }
    public string? TraceNumber { get; private set; }
    public string? MaskedPan { get; private set; }
    public DateTimeOffset? SucceededAtUtc { get; private set; }
    public PaymentFailureReason? FailureReason { get; private set; }
    public PaymentServiceProvider? Psp { get; private set; }
    public User? User { get; private set; }

    private Payment()
    {

    }

    private Payment(
        PaymentPurpose purpose,
        PaymentChannel channel,
        Money amount,
        long userId,
        PaymentDirection direction,
        string serviceId,
        string referenceId,
        long? billId,
        PspCode? pspCode,
        IEventPublisher eventPublisher)
    {
        Guard<InvalidPaymentPurposeException>.IsTrue(!Enum.IsDefined(purpose));
        Guard<InvalidPaymentChannelException>.IsTrue(!Enum.IsDefined(channel));

        Guard<NullEntryException>.AgainstNull(amount);
        Guard<InvalidPaymentAmountException>.IsTrue(amount.Value == 0);

        Guard<InvalidUserIdException>.IsTrue(userId <= 0);
        Guard<InvalidPaymentDirectionException>.IsTrue(!Enum.IsDefined(direction));

        Guard<InvalidServiceIdException>.AgainstNullOrEmpty(serviceId);
        Guard<InvalidReferenceIdException>.AgainstNullOrEmpty(referenceId);

        Guard<MissingPspCodeException>.IsTrue(channel == PaymentChannel.Psp && pspCode == null);
        Guard<UnexpectedPspCodeException>.IsTrue(channel != PaymentChannel.Psp && pspCode != null);
        Guard<InvalidPspCodeException>.IsTrue(pspCode.HasValue && !Enum.IsDefined(pspCode.Value));

        Guard<NullEntryException>.AgainstNull(eventPublisher);

        Purpose = purpose;
        Channel = channel;
        State = PaymentState.Initiated;
        Amount = amount;
        UserId = userId;
        Direction = direction;
        ServiceId = serviceId;
        ReferenceId = referenceId;
        BillId = billId;
        PspCode = pspCode;
        Publisher = eventPublisher;
    }

    public static Payment CreatePspPurchasePayment(
        Money amount,
        long userId,
        string serviceId,
        string referenceId,
        long billId,
        PspCode pspCode,
        IEventPublisher eventPublisher)
    {
        return new Payment(
            PaymentPurpose.Purchase,
            PaymentChannel.Psp,
            amount,
            userId,
            PaymentDirection.Inbound,
            serviceId,
            referenceId,
            billId,
            pspCode,
            eventPublisher);
    }

    public static Payment CreateTopUpPayment(
        Money amount,
        long userId,
        string serviceId,
        string referenceId,
        PspCode pspCode,
        IEventPublisher eventPublisher)
    {
        return new Payment(
            PaymentPurpose.TopUp,
            PaymentChannel.Psp,
            amount,
            userId,
            PaymentDirection.Inbound,
            serviceId,
            referenceId,
            null,
            pspCode,
            eventPublisher);
    }

    public static Payment CreateWalletPurchasePayment(
        Money amount,
        long userId,
        string serviceId,
        string referenceId,
        IEventPublisher eventPublisher)
    {
        return new Payment(
            PaymentPurpose.Purchase,
            PaymentChannel.Wallet,
            amount,
            userId,
            PaymentDirection.Outbound,
            serviceId,
            referenceId,
            null,
            null,
            eventPublisher);
    }

    public async Task MarkTokenRequestFailed(PaymentFailureReason failureReason)
    {
        SetState(PaymentState.Failed);
        FailureReason = failureReason;

        await Publisher.Publish(new PaymentTokenRequestFailedEvent(ReferenceId));
    }

    public async Task MarkTokenReceived(string token)
    {
        SetState(PaymentState.TokenReceived);

        SetToken(token);

        await Publisher.Publish(new PspTokenReceivedEvent(ReferenceId));
    }

    public void StoreCallback(
        string referenceNumber,
        Money redirectedAmount,
        string maskedPan,
        string rrn,
        string traceNumber)
    {
        SetState(PaymentState.CallbackReceived);

        ReferenceNumber = referenceNumber;
        RedirectedAmount = redirectedAmount;
        MaskedPan = maskedPan;
        RRN = rrn;
        TraceNumber = traceNumber;
    }

    public async Task MarkFailed(PaymentFailureReason? failureReason = null)
    {
        SetState(PaymentState.Failed);

        if (failureReason != null)
        {
            FailureReason = failureReason;
        }

        await Publisher.Publish(new PaymentFailedEvent(ReferenceId));
    }

    public void MarkVerifying()
    {
        SetState(PaymentState.Verifying);
    }

    public async Task MarkSucceeded()
    {
        SetState(PaymentState.Succeeded);
        SucceededAtUtc = DateTimeOffset.UtcNow;

        await Publisher.Publish(new PaymentSucceededEvent(ReferenceId));
    }

    public async Task MarkReversed()
    {
        SetState(PaymentState.Reversed);

        await Publisher.Publish(new PaymentReversedEvent(ReferenceId));
    }

    private void SetState(PaymentState newState)
    {
        var isAllowed = State switch
        {
            PaymentState.Initiated when Channel == PaymentChannel.Wallet => newState is PaymentState.Succeeded or PaymentState.Failed,
            PaymentState.Initiated => newState is PaymentState.TokenReceived or PaymentState.Failed,
            PaymentState.TokenReceived => newState is PaymentState.CallbackReceived,
            PaymentState.CallbackReceived => newState is PaymentState.Verifying or PaymentState.Failed,
            PaymentState.Succeeded => false,
            PaymentState.Failed => false,
            PaymentState.Expired => false,
            PaymentState.Reversed => false,
            PaymentState.Verifying => newState is PaymentState.Succeeded or PaymentState.Reversed or PaymentState.Failed,
            _ => throw new InvalidPaymentStateException()
        };

        Guard<InvalidPaymentStateException>.IsFalse(isAllowed);

        State = newState;
    }

    private void SetToken(string token)
    {
        Guard<InvalidTokenException>.AgainstNullOrEmpty(token);

        Token = token;
    }
}
