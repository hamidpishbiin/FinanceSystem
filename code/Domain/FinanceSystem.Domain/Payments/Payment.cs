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

    public PaymentPurpose Purpose { get; private set; }
    public PaymentChannel Channel { get; private set; }
    public PaymentStatus Status { get; private set; }
    public decimal Amount { get; private set; }
    public decimal? RedirectedAmount { get; private set; }
    public long SourceFinanceAccountId { get; private set; }
    public long DestinationFinanceAccountId { get; private set; }
    public string OriginServiceId { get; private set; }
    public string ExternalReferenceId { get; private set; }
    public string ExternalTag { get; private set; }
    public PspCode? PspCode { get; private set; }
    public string? Token { get; private set; }
    public string? RRN { get; private set; }
    public string? RefNum { get; private set; }
    public string? TraceNumber { get; private set; }
    public string? MaskedPan { get; private set; }
    public PspFailureReason? FailureReason { get; private set; }
    public string? RawStatus { get; private set; }
    public string? RawErrorCode { get; private set; }
    public string? RawDescription { get; private set; }
    public DateTimeOffset? VerifiedAtUtc { get; private set; }

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
        string originServiceId,
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

        Guard<InvalidOriginServiceIdException>.AgainstNullOrEmpty(originServiceId);
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
            Status = PaymentStatus.Initiated,
            Amount = amount.Value,
            SourceFinanceAccountId = sourceFinanceAccountId,
            DestinationFinanceAccountId = destinationFinanceAccountId,
            OriginServiceId = originServiceId,
            ExternalReferenceId = externalReferenceId,
            ExternalTag = externalTag,
            PspCode = pspCode
        };

        var paymentCreatedEvent = new PaymentCreatedEvent(payment.ExternalReferenceId, amount.Value);

        await eventPublisher.Publish(paymentCreatedEvent);

        return payment;
    }

    public async Task MarkTokenRequestFailed(
        PspFailureReason failureReason,
        string? rawStatus,
        string? rawErrorCode,
        string? rawDescription)
    {
        Guard<InvalidPaymentStateException>.IsTrue(Channel != PaymentChannel.Psp || Status != PaymentStatus.Initiated);

        Status = PaymentStatus.Failed;
        FailureReason = failureReason;
        RawStatus = rawStatus;
        RawErrorCode = rawErrorCode;
        RawDescription = rawDescription;

        await Publisher.Publish(new PaymentTokenRequestFailedEvent(ExternalReferenceId, Amount));
    }

    public async Task MarkTokenReceived(string token, string ipgUrl)
    {
        Guard<InvalidPaymentStateException>.IsTrue(Channel != PaymentChannel.Psp || Status != PaymentStatus.Initiated);
        Guard<InvalidTokenException>.AgainstNullOrEmpty(token);

        Token = token;
        Status = PaymentStatus.TokenReceived;

        await Publisher.Publish(new PspTokenReceivedEvent(ExternalReferenceId, Amount, ipgUrl));
    }
}
