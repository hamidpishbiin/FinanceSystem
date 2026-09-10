using FinanceSystem.Domain.AccountEntries;
using FinanceSystem.Domain.AccountEntries.Enums;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;

namespace FinanceSystem.Domain.Tests.Unit.AccountEntryUnitTests;

public class AccountEntryBuilder
{
    public const long DefaultPaymentId = 12;
    public const long DefaultAccountId = 13;
    public const decimal DefaultAmountRial = 110;
    public const EntryDirection DefaultDirection = EntryDirection.In;

    private long PaymentId { get; set; } = DefaultPaymentId;
    private long AccountId { get; set; } = DefaultAccountId;
    private Money Amount { get; set; } = new(DefaultAmountRial, Currency.Rial);
    private EntryDirection Direction { get; set; } = DefaultDirection;

    public Task<AccountEntry> Build()
    {
        return AccountEntry.Create(PaymentId, AccountId, Amount, Direction);
    }

    public AccountEntryBuilder WithPaymentId(long paymentId)
    {
        PaymentId = paymentId;
        return this;
    }

    public AccountEntryBuilder WithAccountId(long accountId)
    {
        AccountId = accountId;
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
