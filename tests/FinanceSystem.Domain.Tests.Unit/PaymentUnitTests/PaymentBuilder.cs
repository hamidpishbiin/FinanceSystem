using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using NSubstitute;
using Shared.Core.Events;

namespace FinanceSystem.Domain.Tests.Unit.PaymentUnitTests;

public class PaymentBuilder
{
    public const PaymentPurpose DefaultPurpose = PaymentPurpose.Purchase;
    public const PaymentChannel DefaultChannel = PaymentChannel.Wallet;
    public const decimal DefaultAmount = 250_000;
    public const long DefaultFinanceAccountId = 22;
    public const PaymentDirection DefaultDirection = PaymentDirection.Inbound;
    public const string DefaultServiceId = "checkout-service";
    public const string DefaultReferenceId = "ext-ref-9931";
    public const PspCode DefaultPspCode = Domain.PaymentServiceProviders.Enums.PspCode.Saman;

    private PaymentPurpose Purpose { get; set; } = DefaultPurpose;
    private PaymentChannel Channel { get; set; } = DefaultChannel;
    private Money Amount { get; set; } = new(DefaultAmount);
    private long FinanceAccountId { get; set; } = DefaultFinanceAccountId;
    private PaymentDirection Direction { get; set; } = DefaultDirection;
    private string ServiceId { get; set; } = DefaultServiceId;
    private string ReferenceId { get; set; } = DefaultReferenceId;
    private PspCode? PspCode { get; set; }

    public IEventPublisher EventPublisher { get; private set; } = Substitute.For<IEventPublisher>();

    public async Task<Payment> Build()
    {
        return await Payment.Create(
            Purpose,
            Channel,
            Amount,
            FinanceAccountId,
            Direction,
            ServiceId,
            ReferenceId,
            PspCode,
            EventPublisher);
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

    public PaymentBuilder WithFinanceAccountId(long financeAccountId)
    {
        FinanceAccountId = financeAccountId;
        return this;
    }

    public PaymentBuilder WithDirection(PaymentDirection direction)
    {
        Direction = direction;
        return this;
    }

    public PaymentBuilder WithServiceId(string? serviceId)
    {
        ServiceId = serviceId!;
        return this;
    }

    public PaymentBuilder WithReferenceId(string? referenceId)
    {
        ReferenceId = referenceId!;
        return this;
    }

    public PaymentBuilder WithPspCode(PspCode? pspCode)
    {
        PspCode = pspCode;
        return this;
    }

    public PaymentBuilder WithEventPublisher(IEventPublisher eventPublisher)
    {
        EventPublisher = eventPublisher;
        return this;
    }
}
