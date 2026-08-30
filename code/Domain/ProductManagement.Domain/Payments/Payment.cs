using ProductManagement.Domain.Contract.Payments;
using ProductManagement.Domain.Payments.Enums;
using ProductManagement.Domain.Payments.Exceptions;

namespace ProductManagement.Domain.Payments;

public sealed class Payment : EntityBase<long>, IAggregateRoot
{

    public IEventPublisher Publisher { get; set; }

    public string IdempotencyKey { get; private set; } = string.Empty;
    public PaymentPurpose Purpose { get; private set; }
    public PaymentChannel Channel { get; private set; }
    public decimal AmountRial { get; private set; }
    public long SourceAccountId { get; private set; }
    public long DestinationAccountId { get; private set; }
    public string OriginServiceId { get; private set; } = string.Empty;
    public string ExternalReferenceId { get; private set; } = string.Empty;
    public string ExternalTag { get; private set; } = string.Empty;
    public long? BankPaymentDetailId { get; private set; }

    private Payment()
    {

    }

    public static async Task<Payment> Create(
        string idempotencyKey,
        PaymentPurpose purpose,
        PaymentChannel channel,
        Money amount,
        long sourceAccountId,
        long destinationAccountId,
        string originServiceId,
        string externalReferenceId,
        string externalTag,
        long? bankPaymentDetailId,
        IEventPublisher eventPublisher)
    {
        Guard<IncoherentPaymentException>.AgainstNullOrEmpty(idempotencyKey);
        Guard<IncoherentPaymentException>.SmallerThan(sourceAccountId, 0);
        Guard<IncoherentPaymentException>.SmallerThan(destinationAccountId, 0);
        Guard<IncoherentPaymentException>.AgainstNullOrEmpty(originServiceId);
        Guard<IncoherentPaymentException>.AgainstNullOrEmpty(externalReferenceId);
        Guard<IncoherentPaymentException>.AgainstNullOrEmpty(externalTag);
        Guard<IncoherentPaymentException>.IsTrue(amount.Currency != Currency.Rial);
        Guard<IncoherentPaymentException>.IsTrue(channel == PaymentChannel.Bank && bankPaymentDetailId == null);

        var payment = new Payment()
        {
            IdempotencyKey = idempotencyKey,
            Purpose = purpose,
            Channel = channel,
            AmountRial = amount.Value,
            SourceAccountId = sourceAccountId,
            DestinationAccountId = destinationAccountId,
            OriginServiceId = originServiceId,
            ExternalReferenceId = externalReferenceId,
            ExternalTag = externalTag,
            BankPaymentDetailId = bankPaymentDetailId,
            Publisher = eventPublisher
        };

        var paymentCreatedEvent = new PaymentCreatedEvent()
        {
            IdempotencyKey = payment.IdempotencyKey,
            AmountRial = amount.Value
        };

        await eventPublisher.Publish(paymentCreatedEvent);

        return payment;
    }
}



