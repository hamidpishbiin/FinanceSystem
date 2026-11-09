using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using NSubstitute;
using Shared.Core.Events;

namespace FinanceSystem.Domain.Tests.Unit.PaymentUnitTests;

public class PaymentBuilder
{
    public const decimal DefaultAmount = 250_000;
    public const long DefaultUserId = 22;
    public const string DefaultServiceId = "checkout-service";
    public const string DefaultReferenceId = "ext-ref-9931";
    public const long DefaultBillId = 41;
    public const PspCode DefaultPspCode = Domain.PaymentServiceProviders.Enums.PspCode.Saman;

    private Money Amount { get; set; } = new(DefaultAmount);
    private long UserId { get; set; } = DefaultUserId;
    private string ServiceId { get; set; } = DefaultServiceId;
    private string ReferenceId { get; set; } = DefaultReferenceId;
    private long BillId { get; set; } = DefaultBillId;
    private PspCode PspCode { get; set; } = DefaultPspCode;

    public IEventPublisher EventPublisher { get; } = Substitute.For<IEventPublisher>();

    public Payment BuildTopUp()
    {
        return Payment.CreateTopUpPayment(
            Amount,
            UserId,
            ServiceId,
            ReferenceId,
            PspCode);
    }

    public Payment BuildPspPurchase()
    {
        return Payment.CreatePspPurchasePayment(
            Amount,
            UserId,
            ServiceId,
            ReferenceId,
            BillId,
            PspCode);
    }

    public PaymentBuilder WithAmount(Money amount)
    {
        Amount = amount;
        return this;
    }

    public PaymentBuilder WithUserId(long userId)
    {
        UserId = userId;
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

    public PaymentBuilder WithBillId(long billId)
    {
        BillId = billId;
        return this;
    }

    public PaymentBuilder WithPspCode(PspCode pspCode)
    {
        PspCode = pspCode;
        return this;
    }
}
