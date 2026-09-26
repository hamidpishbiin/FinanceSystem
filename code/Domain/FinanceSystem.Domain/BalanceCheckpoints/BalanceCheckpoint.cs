using FinanceSystem.Domain.BalanceCheckpoints.Exceptions;
using FinanceSystem.Domain.AccountEntries;
using FinanceSystem.Domain.FinanceAccounts;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.BalanceCheckpoints;

public class BalanceCheckpoint : EntityBase<long>
{
    public long FinanceAccountId { get; private set; }
    public long UpToEntryId { get; private set; }
    public decimal Balance { get; private set; }

    public FinanceAccount? FinanceAccount { get; private set; }
    public AccountEntry? UpToEntry { get; private set; }

    private BalanceCheckpoint()
    {
    }

    public BalanceCheckpoint(
        long financeAccountId,
        long upToEntryId,
        Money balance)
    {
        Guard<InvalidFinanceAccountIdException>.IsTrue(financeAccountId <= 0);
        Guard<InvalidUpToEntryIdException>.IsTrue(upToEntryId <= 0);
        Guard<NullEntryException>.AgainstNull(balance);

        FinanceAccountId = financeAccountId;
        UpToEntryId = upToEntryId;
        Balance = balance.Value;
    }
}
