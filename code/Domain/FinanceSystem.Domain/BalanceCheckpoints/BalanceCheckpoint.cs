using FinanceSystem.Domain.BalanceCheckpoints.Exceptions;
using FinanceSystem.Domain.AccountEntries;
using FinanceSystem.Domain.Accounts;

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

    public static BalanceCheckpoint Create(
        long accountId,
        long upToEntryId,
        decimal balanceRial)
    {
        Guard<InvalidAccountIdException>.IsTrue(accountId <= 0);
        Guard<InvalidUpToEntryIdException>.IsTrue(upToEntryId <= 0);

        return new BalanceCheckpoint()
        {
            AccountId = accountId,
            UpToEntryId = upToEntryId,
            BalanceRial = balanceRial
        };
    }
}
