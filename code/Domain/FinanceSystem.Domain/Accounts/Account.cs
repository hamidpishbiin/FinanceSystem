using FinanceSystem.Domain.AccountEntries;
using FinanceSystem.Domain.AccountEntries.Enums;
using FinanceSystem.Domain.Accounts.Enums;
using FinanceSystem.Domain.Accounts.Exceptions;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.Accounts;

public class Account : EntityBase<long>
{
    public AccountType Type { get; private set; }
    public AccountStatus Status { get; private set; }
    public long UserId { get; private set; }
    public decimal CachedBalance { get; private set; }
    public DateTimeOffset BalanceCalculatedAt { get; private set; }
    public bool AllowNegativeBalance { get; private set; }

    private Account()
    {
    }

    public static async Task<Account> Create(
        AccountType type,
        AccountStatus status,
        long ownerId,
        Money cachedBalance,
        DateTimeOffset balanceCalculatedAt,
        bool allowNegativeBalance)
    {
        Guard<InvalidIdException>.IsTrue(ownerId <= 0);
        Guard<NullEntryException>.AgainstNull(cachedBalance);

        return new Account()
        {
            Type = type,
            Status = status,
            UserId = ownerId,
            CachedBalance = cachedBalance.Value,
            BalanceCalculatedAt = balanceCalculatedAt,
            AllowNegativeBalance = allowNegativeBalance
        };
    }

    public void ApplyEntry(AccountEntry entry, DateTimeOffset occurredAt)
    {
        Guard<NullEntryException>.AgainstNull(entry);
        Guard<EntryAccountMismatchException>.IsTrue(entry.AccountId != Id);
        Guard<AccountClosedException>.IsTrue(Status == AccountStatus.Closed);
        Guard<AccountDirectionNotAllowedException>.IsFalse(Allows(entry.Direction));

        var newBalance = CachedBalance + entry.SignedAmount;

        Guard<InsufficientBalanceException>.IsTrue(newBalance < 0 && !AllowNegativeBalance);

        CachedBalance = newBalance;
        BalanceCalculatedAt = occurredAt;
    }

    private bool Allows(EntryDirection direction) => Status switch
    {
        AccountStatus.Active => true,
        AccountStatus.InboundFrozen => direction == EntryDirection.Out,
        AccountStatus.OutboundFrozen => direction == EntryDirection.In,
        AccountStatus.CompletelyFrozen => false,
        AccountStatus.Closed => false,
        _ => false
    };
}
