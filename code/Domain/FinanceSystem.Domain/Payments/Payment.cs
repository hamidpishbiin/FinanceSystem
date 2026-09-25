using FinanceSystem.Domain.Contract.Payments;
using FinanceSystem.Domain.Accounts;
using FinanceSystem.Domain.RequestsToPay;
using FinanceSystem.Domain.Payments.Enums;
using FinanceSystem.Domain.Payments.Exceptions;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.Payments;

public sealed class Payment : EntityBase<long>, IAggregateRoot
{

    public IEventPublisher Publisher { get; set; }

    public PaymentPurpose Purpose { get; private set; }
    public PaymentChannel Channel { get; private set; }
    public decimal Amount { get; private set; }
    public long SourceAccountId { get; private set; }
    public long DestinationAccountId { get; private set; }
    public string OriginServiceId { get; private set; }
    public string ExternalReferenceId { get; private set; }
    public string ExternalTag { get; private set; }
    public long? RequestToPayId { get; private set; }

    public Account? SourceAccount { get; private set; }
    public Account? DestinationAccount { get; private set; }
    public RequestToPay? RequestToPay { get; private set; }

    private Payment()
    {

    }

    public static async Task<Payment> Create(
        PaymentPurpose purpose,
        PaymentChannel channel,
        Money amount,
        long sourceAccountId,
        long destinationAccountId,
        string originServiceId,
        string externalReferenceId,
        string externalTag,
        long? requestToPayId,
        IEventPublisher eventPublisher)
    {
        Guard<InvalidPaymentPurposeException>.IsFalse(Enum.IsDefined(purpose));
        Guard<InvalidPaymentChannelException>.IsFalse(Enum.IsDefined(channel));

        Guard<NullEntryException>.AgainstNull(amount);
        Guard<InvalidPaymentAmountException>.IsTrue(amount.Value <= 0);

        Guard<InvalidSourceAccountIdException>.IsTrue(sourceAccountId <= 0);
        Guard<InvalidDestinationAccountIdException>.IsTrue(destinationAccountId <= 0);
        Guard<SameSourceAndDestinationAccountException>.IsTrue(sourceAccountId == destinationAccountId);

        Guard<InvalidOriginServiceIdException>.AgainstNullOrEmpty(originServiceId);
        Guard<InvalidExternalReferenceIdException>.AgainstNullOrEmpty(externalReferenceId);
        Guard<InvalidExternalTagException>.AgainstNullOrEmpty(externalTag);

        Guard<MissingRequestToPayException>.IsTrue(channel == PaymentChannel.Psp && requestToPayId == null);
        Guard<UnexpectedRequestToPayException>.IsTrue(channel != PaymentChannel.Psp && requestToPayId != null);

        Guard<NullEntryException>.AgainstNull(eventPublisher);

        var payment = new Payment()
        {
            Purpose = purpose,
            Channel = channel,
            Amount = amount.Value,
            SourceAccountId = sourceAccountId,
            DestinationAccountId = destinationAccountId,
            OriginServiceId = originServiceId,
            ExternalReferenceId = externalReferenceId,
            ExternalTag = externalTag,
            RequestToPayId = requestToPayId,
            Publisher = eventPublisher
        };

        var paymentCreatedEvent = new PaymentCreatedEvent(payment.ExternalReferenceId, amount.Value);

        await eventPublisher.Publish(paymentCreatedEvent);

        return payment;
    }
}



