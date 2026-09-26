using FinanceSystem.Domain.AccountEntries;
using FinanceSystem.Domain.AccountEntries.Enums;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;

namespace FinanceSystem.Domain.Tests.Unit.AccountEntryUnitTests;

public class AccountEntryBuilder
{
    public const long DefaultPaymentId = 12;
    public const long DefaultFinanceAccountId = 13;
    public const decimal DefaultAmount = 110;
    public const EntryDirection DefaultDirection = EntryDirection.In;

    private long PaymentId { get; set; } = DefaultPaymentId;
    private long FinanceAccountId { get; set; } = DefaultFinanceAccountId;
    private Money Amount { get; set; } = new(DefaultAmount);
    private EntryDirection Direction { get; set; } = DefaultDirection;

    public AccountEntry Build()
    {
        return new AccountEntry(PaymentId, FinanceAccountId, Amount, Direction);
    }

    public AccountEntryBuilder WithPaymentId(long paymentId)
    {
        PaymentId = paymentId;
        return this;
    }

    public AccountEntryBuilder WithFinanceAccountId(long financeAccountId)
    {
        FinanceAccountId = financeAccountId;
        return this;
    }

    public AccountEntryBuilder WithAmount(Money amount)
    {
        Amount = amount;
        return this;
    }

    public AccountEntryBuilder WithDirection(EntryDirection direction)
    {
        Direction = direction;
        return this;
    }
}
