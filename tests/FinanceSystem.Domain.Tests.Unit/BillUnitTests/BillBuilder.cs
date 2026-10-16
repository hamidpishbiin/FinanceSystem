using FinanceSystem.Domain.Bills;
using FinanceSystem.Domain.Payments;
using NSubstitute;
using Shared.Core.Events;

namespace FinanceSystem.Domain.Tests.Unit.BillUnitTests;

public class BillBuilder
{
    public IEventPublisher Publisher { get; private set; } = Substitute.For<IEventPublisher>();
    public const long DefaultServiceId = 12;
    public const long DefaultReferenceId = 1234;
    public const long DefaultFinanceAccountId = 78645;
    public const decimal DefaultAmount = 8765;

    private long ServiceId { get; set; } = DefaultServiceId;
    private long ReferenceId { get; set; } = DefaultReferenceId;
    private long FinanceAccountId { get; set; } = DefaultFinanceAccountId;
    private decimal Amount { get; set; } = DefaultAmount;

    public Bill Build()
    {
        return new Bill(
            ServiceId,
            ReferenceId,
            FinanceAccountId,
            new Money(Amount));
    }

    public BillBuilder WithServiceId(long serviceId)
    {
        ServiceId = serviceId;
        return this;
    }

    public BillBuilder WithReferenceId(long referenceId)
    {
        ReferenceId = referenceId;
        return this;
    }

    public BillBuilder WithFinanceAccountId(long financeAccountId)
    {
        FinanceAccountId = financeAccountId;
        return this;
    }

    public BillBuilder WithAmount(Money amount)
    {
        Amount = amount.Value;
        return this;
    }
}
