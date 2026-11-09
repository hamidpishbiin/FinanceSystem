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
        PspCode? pspCode)
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
    }

    public static Payment CreatePspPurchasePayment(
        Money amount,
        long userId,
        string serviceId,
        string referenceId,
        long billId,
        PspCode pspCode)
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
            pspCode);
    }

    public static Payment CreateTopUpPayment(
        Money amount,
        long userId,
        string serviceId,
        string referenceId,
        PspCode pspCode)
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
            pspCode);
    }

    public static Payment CreateWalletPurchasePayment(
        Money amount,
        long userId,
        string serviceId,
        string referenceId)
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
            null);
    }

    public async Task MarkTokenRequestFailed(PaymentFailureReason failureReason, IEventPublisher publisher)
    {
        SetState(PaymentState.Failed);
        FailureReason = failureReason;

        await publisher.Publish(new PaymentTokenRequestFailed(ReferenceId));
    }

    public async Task MarkTokenReceived(string token, IEventPublisher publisher)
    {
        SetState(PaymentState.TokenReceived);

        SetToken(token);

        await publisher.Publish(new PspTokenReceived(ReferenceId));
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

    public async Task MarkFailed(PaymentFailureReason? failureReason, IEventPublisher publisher)
    {
        SetState(PaymentState.Failed);

        if (failureReason != null)
        {
            FailureReason = failureReason;
        }

        await publisher.Publish(new PaymentFailed(ReferenceId));
    }

    public void MarkVerifying()
    {
        SetState(PaymentState.Verifying);
    }

    public async Task MarkSucceeded(IEventPublisher publisher)
    {
        SetState(PaymentState.Succeeded);
        SucceededAtUtc = DateTimeOffset.UtcNow;

        await publisher.Publish(new PaymentSucceeded(ReferenceId));
    }

    public async Task MarkReversed(IEventPublisher publisher)
    {
        SetState(PaymentState.Reversed);

        await publisher.Publish(new PaymentReversed(ReferenceId));
    }

    public void EnsureCallbackValidity(PspCode pspCode, string token, Money redirectAmount)
    {
        Guard<PspCodeMismatchException>.IsTrue(PspCode != pspCode);
        Guard<CallbackTokenMismatchException>.IsTrue(Token != token);
        Guard<InconsistentRedirectAmountException>.IsTrue(Amount != redirectAmount);
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
