using FinanceSystem.Domain.BalanceCheckpoints.Exceptions;
using FinanceSystem.Domain.AccountEntries;
using FinanceSystem.Domain.Accounts;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.BalanceCheckpoints;

public class BalanceCheckpoint : EntityBase<long>
{
    public long AccountId { get; private set; }
    public long UpToEntryId { get; private set; }
    public decimal BalanceRial { get; private set; }

    public Account? Account { get; private set; }
    public AccountEntry? UpToEntry { get; private set; }

    private BalanceCheckpoint()
    {
    }

    public static async Task<BalanceCheckpoint> Create(
        long accountId,
        long upToEntryId,
        Money balance)
    {
        Guard<InvalidAccountIdException>.IsTrue(accountId <= 0);
        Guard<InvalidUpToEntryIdException>.IsTrue(upToEntryId <= 0);
        Guard<NullEntryException>.AgainstNull(balance);
        Guard<InvalidMoneyCurrencyException>.IsTrue(balance.Currency != Currency.Rial);

        return new BalanceCheckpoint()
        {
            AccountId = accountId,
            UpToEntryId = upToEntryId,
            BalanceRial = balance.Value
        };
    }
}
