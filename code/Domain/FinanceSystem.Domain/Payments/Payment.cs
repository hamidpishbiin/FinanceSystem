using FinanceSystem.Domain.FinanceAccounts.Exceptions;
using FinanceSystem.Domain.Contract.Payments;
using FinanceSystem.Domain.FinanceAccounts;
using FinanceSystem.Domain.PaymentServiceProviders;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Exceptions;
using FinanceSystem.Domain.Payments.Enums;
using FinanceSystem.Domain.Payments.Exceptions;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.Payments;

public sealed class Payment : EntityBase<long>, IAggregateRoot
{

    public IEventPublisher Publisher { get; set; }

    public decimal Amount { get; private set; }
    public PaymentPurpose Purpose { get; private set; }
    public PaymentChannel Channel { get; private set; }
    public PaymentState State { get; private set; }

    public long FinanceAccountId { get; private set; }
    public PaymentDirection Direction { get; private set; }
    public string ServiceId { get; private set; }
    public string ReferenceId { get; private set; }
    public long? BillId { get; private set; }
    public PspCode? PspCode { get; private set; }
    public string? Token { get; private set; }
    public string? RRN { get; private set; }
    public decimal? RedirectedAmount { get; private set; }
    public string? ReferenceNumber { get; private set; }
    public string? TraceNumber { get; private set; }
    public string? MaskedPan { get; private set; }
    public string? Status { get; private set; }
    public int? StatusCode { get; private set; }
    public string? RawDescription { get; private set; }
    public DateTimeOffset? SucceededAtUtc { get; private set; }
    public PaymentFailureReason? FailureReason { get; private set; }
    public FinanceAccount? FinanceAccount { get; private set; }
    public PaymentServiceProvider? Psp { get; private set; }

    private Payment()
    {

    }

    private Payment(
        PaymentPurpose purpose,
        PaymentChannel channel,
        Money amount,
        long financeAccountId,
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

        Guard<InvalidFinanceAccountIdException>.IsTrue(financeAccountId <= 0);
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
        Amount = amount.Value;
        FinanceAccountId = financeAccountId;
        Direction = direction;
        ServiceId = serviceId;
        ReferenceId = referenceId;
        BillId = billId;
        PspCode = pspCode;
        Publisher = eventPublisher;
    }

    public static Payment CreatePspPurchasePayment(
        Money amount,
        long financeAccountId,
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
            financeAccountId,
            PaymentDirection.Inbound,
            serviceId,
            referenceId,
            billId,
            pspCode,
            eventPublisher);
    }

    public static Payment CreateTopUpPayment(
        Money amount,
        long financeAccountId,
        string serviceId,
        string referenceId,
        PspCode pspCode,
        IEventPublisher eventPublisher)
    {
        return new Payment(
            PaymentPurpose.TopUp,
            PaymentChannel.Psp,
            amount,
            financeAccountId,
            PaymentDirection.Inbound,
            serviceId,
            referenceId,
            null,
            pspCode,
            eventPublisher);
    }

    public async Task MarkTokenRequestFailed(
        PaymentFailureReason failureReason,
        string? rawStatus,
        int? rawStatusCode,
        string? rawDescription)
    {
        Guard<InvalidPaymentStateException>.IsTrue(Channel != PaymentChannel.Psp);

        SetState(PaymentState.Failed);
        FailureReason = failureReason;
        Status = rawStatus;
        StatusCode = rawStatusCode;
        RawDescription = rawDescription;

        await Publisher.Publish(new PaymentTokenRequestFailedEvent(ReferenceId));
    }

    public async Task MarkTokenReceived(string token, string ipgUrl)
    {
        Guard<InvalidPaymentStateException>.IsTrue(Channel != PaymentChannel.Psp);
        Guard<InvalidTokenException>.AgainstNullOrEmpty(token);

        SetState(PaymentState.TokenReceived);
        Token = token;

        await Publisher.Publish(new PspTokenReceivedEvent(ReferenceId, ipgUrl));
    }

    public void StoreCallback(
        string status,
        int statusCode,
        string referenceNumber,
        decimal redirectedAmount,
        string maskedPan,
        string rrn,
        string traceNumber)
    {
        Guard<InvalidRawStatusException>.AgainstNullOrEmpty(status);

        SetState(PaymentState.CallbackReceived);

        Status = status;
        StatusCode = statusCode;
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
}
