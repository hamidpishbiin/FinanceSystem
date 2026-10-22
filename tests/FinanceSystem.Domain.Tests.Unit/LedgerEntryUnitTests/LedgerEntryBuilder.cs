using FinanceSystem.Domain.LedgerEntries;
using FinanceSystem.Domain.LedgerEntries.Enums;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;

namespace FinanceSystem.Domain.Tests.Unit.LedgerEntryUnitTests;

public class LedgerEntryBuilder
{
    public const long DefaultPaymentId = 12;
    public const long DefaultFinanceAccountId = 13;
    public const decimal DefaultAmount = 110;
    public const EntryDirection DefaultDirection = EntryDirection.In;

    private long PaymentId { get; set; } = DefaultPaymentId;
    private long FinanceAccountId { get; set; } = DefaultFinanceAccountId;
    private Money Amount { get; set; } = new(DefaultAmount);
    private EntryDirection Direction { get; set; } = DefaultDirection;

    public LedgerEntry Build()
    {
        return new LedgerEntry(PaymentId, FinanceAccountId, Amount, Direction);
    }

    public LedgerEntryBuilder WithPaymentId(long paymentId)
    {
        PaymentId = paymentId;
        return this;
    }

    public LedgerEntryBuilder WithFinanceAccountId(long financeAccountId)
    {
        FinanceAccountId = financeAccountId;
        return this;
    }

    public LedgerEntryBuilder WithAmount(Money amount)
    {
        Amount = amount;
        return this;
    }

    public LedgerEntryBuilder WithDirection(EntryDirection direction)
    {
        Direction = direction;
        return this;
    }
}
