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

    public long SourceFinanceAccountId { get; private set; }
    public long DestinationFinanceAccountId { get; private set; }
    public string ExternalServiceId { get; private set; }
    public string ExternalReferenceId { get; private set; }
    public string ExternalTag { get; private set; }
    public PspCode? PspCode { get; private set; }
    public string? Token { get; private set; }
    public string? RRN { get; private set; }
    public decimal? RedirectedAmount { get; private set; }
    public string? ReferenceNumber { get; private set; }
    public string? TraceNumber { get; private set; }
    public string? MaskedPan { get; private set; }
    public string? RawStatus { get; private set; }
    public int? RawStatusCode { get; private set; }
    public string? RawDescription { get; private set; }
    public DateTimeOffset? SucceededAtUtc { get; private set; }
    public PspFailureReason? FailureReason { get; private set; }
    public FinanceAccount? SourceFinanceAccount { get; private set; }
    public FinanceAccount? DestinationFinanceAccount { get; private set; }
    public PaymentServiceProvider? Psp { get; private set; }

    private Payment()
    {

    }

    public static async Task<Payment> Create(
        PaymentPurpose purpose,
        PaymentChannel channel,
        Money amount,
        long sourceFinanceAccountId,
        long destinationFinanceAccountId,
        string externalServiceId,
        string externalReferenceId,
        string externalTag,
        PspCode? pspCode,
        IEventPublisher eventPublisher)
    {
        Guard<InvalidPaymentPurposeException>.IsFalse(Enum.IsDefined(purpose));
        Guard<InvalidPaymentChannelException>.IsFalse(Enum.IsDefined(channel));

        Guard<NullEntryException>.AgainstNull(amount);
        Guard<InvalidPaymentAmountException>.IsTrue(amount.Value <= 0);

        Guard<InvalidSourceFinanceAccountIdException>.IsTrue(sourceFinanceAccountId <= 0);
        Guard<InvalidDestinationFinanceAccountIdException>.IsTrue(destinationFinanceAccountId <= 0);
        Guard<SameSourceAndDestinationFinanceAccountException>.IsTrue(sourceFinanceAccountId == destinationFinanceAccountId);

        Guard<InvalidExternalServiceIdException>.AgainstNullOrEmpty(externalServiceId);
        Guard<InvalidExternalReferenceIdException>.AgainstNullOrEmpty(externalReferenceId);
        Guard<InvalidExternalTagException>.AgainstNullOrEmpty(externalTag);

        Guard<MissingPspCodeException>.IsTrue(channel == PaymentChannel.Psp && pspCode == null);
        Guard<UnexpectedPspCodeException>.IsTrue(channel != PaymentChannel.Psp && pspCode != null);
        Guard<InvalidPspCodeException>.IsTrue(pspCode.HasValue && !Enum.IsDefined(pspCode.Value));

        Guard<NullEntryException>.AgainstNull(eventPublisher);

        var payment = new Payment()
        {
            Purpose = purpose,
            Channel = channel,
            State = PaymentState.Initiated,
            Amount = amount.Value,
            SourceFinanceAccountId = sourceFinanceAccountId,
            DestinationFinanceAccountId = destinationFinanceAccountId,
            ExternalServiceId = externalServiceId,
            ExternalReferenceId = externalReferenceId,
            ExternalTag = externalTag,
            PspCode = pspCode,
            Publisher = eventPublisher
        };

        await eventPublisher.Publish(new PaymentCreatedEvent(payment.ExternalReferenceId));

        return payment;
    }

    public async Task MarkTokenRequestFailed(
        PspFailureReason failureReason,
        string? rawStatus,
        int? rawStatusCode,
        string? rawDescription)
    {
        Guard<InvalidPaymentStateException>.IsTrue(Channel != PaymentChannel.Psp || State != PaymentState.Initiated);

        State = PaymentState.Failed;
        FailureReason = failureReason;
        RawStatus = rawStatus;
        RawStatusCode = rawStatusCode;
        RawDescription = rawDescription;

        await Publisher.Publish(new PaymentTokenRequestFailedEvent(ExternalReferenceId));
    }

    public async Task MarkTokenReceived(string token, string ipgUrl)
    {
        Guard<InvalidPaymentStateException>.IsTrue(Channel != PaymentChannel.Psp || State != PaymentState.Initiated);
        Guard<InvalidTokenException>.AgainstNullOrEmpty(token);

        Token = token;
        State = PaymentState.TokenReceived;

        await Publisher.Publish(new PspTokenReceivedEvent(ExternalReferenceId, ipgUrl));
    }

    public void StoreCallback(
        string rawStatus,
        int? rawStatusCode,
        string? referenceNumber,
        decimal? redirectedAmount,
        string? maskedPan,
        string? rrn,
        string? traceNumber)
    {
        Guard<InvalidPaymentStateException>.IsTrue(State != PaymentState.TokenReceived);
        Guard<InvalidRawStatusException>.AgainstNullOrEmpty(rawStatus);

        RawStatus = rawStatus;
        RawStatusCode = rawStatusCode;
        ReferenceNumber = string.IsNullOrWhiteSpace(referenceNumber) ? null : referenceNumber;
        RedirectedAmount = redirectedAmount;
        MaskedPan = maskedPan;
        RRN = rrn;
        TraceNumber = traceNumber;
        State = PaymentState.CallbackReceived;
    }

    public async Task MarkFailed(PspFailureReason failureReason)
    {
        Guard<InvalidPaymentStateException>.IsTrue(State is not (PaymentState.CallbackReceived or PaymentState.Verifying));

        State = PaymentState.Failed;
        FailureReason = failureReason;

        await Publisher.Publish(new PaymentFailedEvent(ExternalReferenceId));
    }

    public void MarkVerifying()
    {
        Guard<InvalidPaymentStateException>.IsTrue(State != PaymentState.CallbackReceived);

        State = PaymentState.Verifying;
    }

    public async Task MarkSucceeded()
    {
        Guard<InvalidPaymentStateException>.IsTrue(State != PaymentState.Verifying);

        State = PaymentState.Succeeded;
        SucceededAtUtc = DateTimeOffset.UtcNow;

        await Publisher.Publish(new PaymentSucceededEvent(ExternalReferenceId));
    }

    public async Task MarkReversed()
    {
        Guard<InvalidPaymentStateException>.IsTrue(State != PaymentState.Verifying);

        State = PaymentState.Reversed;

        await Publisher.Publish(new PaymentReversedEvent(ExternalReferenceId));
    }
}
