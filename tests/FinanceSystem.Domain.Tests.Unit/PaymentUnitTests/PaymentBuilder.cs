using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using NSubstitute;
using Shared.Core.Events;

namespace FinanceSystem.Domain.Tests.Unit.PaymentUnitTests;

public class PaymentBuilder
{
    public const string DefaultIdempotencyKey = "b2f1c7d0-idem-key";
    public const PaymentPurpose DefaultPurpose = PaymentPurpose.Purchase;
    public const PaymentChannel DefaultChannel = PaymentChannel.Wallet;
    public const decimal DefaultAmountRial = 250_000;
    public const long DefaultSourceAccountId = 11;
    public const long DefaultDestinationAccountId = 22;
    public const string DefaultOriginServiceId = "checkout-service";
    public const string DefaultExternalReferenceId = "ext-ref-9931";
    public const string DefaultExternalTag = "order:9931";
    public const long DefaultBankPaymentDetailId = 77;

    private string IdempotencyKey { get; set; } = DefaultIdempotencyKey;
    private PaymentPurpose Purpose { get; set; } = DefaultPurpose;
    private PaymentChannel Channel { get; set; } = DefaultChannel;
    private Money Amount { get; set; } = new(DefaultAmountRial, Currency.Rial);
    private long SourceAccountId { get; set; } = DefaultSourceAccountId;
    private long DestinationAccountId { get; set; } = DefaultDestinationAccountId;
    private string OriginServiceId { get; set; } = DefaultOriginServiceId;
    private string ExternalReferenceId { get; set; } = DefaultExternalReferenceId;
    private string ExternalTag { get; set; } = DefaultExternalTag;
    private long? BankPaymentDetailId { get; set; }

    public IEventPublisher EventPublisher { get; private set; } = Substitute.For<IEventPublisher>();

    public async Task<Payment> Build()
    {
        return await Payment.Create(
            IdempotencyKey,
            Purpose,
            Channel,
            Amount,
            SourceAccountId,
            DestinationAccountId,
            OriginServiceId,
            ExternalReferenceId,
            ExternalTag,
            BankPaymentDetailId,
            EventPublisher);
    }

    public PaymentBuilder WithIdempotencyKey(string? idempotencyKey)
    {
        IdempotencyKey = idempotencyKey!;
        return this;
    }

    public PaymentBuilder WithPurpose(PaymentPurpose purpose)
    {
        Purpose = purpose;
        return this;
    }

    public PaymentBuilder WithChannel(PaymentChannel channel)
    {
        Channel = channel;
        return this;
    }

    public PaymentBuilder WithAmount(Money amount)
    {
        Amount = amount;
        return this;
    }

    public PaymentBuilder WithSourceAccountId(long sourceAccountId)
    {
        SourceAccountId = sourceAccountId;
        return this;
    }

    public PaymentBuilder WithDestinationAccountId(long destinationAccountId)
    {
        DestinationAccountId = destinationAccountId;
        return this;
    }

    public PaymentBuilder WithOriginServiceId(string? originServiceId)
    {
        OriginServiceId = originServiceId!;
        return this;
    }

    public PaymentBuilder WithExternalReferenceId(string? externalReferenceId)
    {
        ExternalReferenceId = externalReferenceId!;
        return this;
    }

    public PaymentBuilder WithExternalTag(string? externalTag)
    {
        ExternalTag = externalTag!;
        return this;
    }

    public PaymentBuilder WithBankPaymentDetailId(long? bankPaymentDetailId)
    {
        BankPaymentDetailId = bankPaymentDetailId;
        return this;
    }

    public PaymentBuilder WithEventPublisher(IEventPublisher eventPublisher)
    {
        EventPublisher = eventPublisher;
        return this;
    }
}
