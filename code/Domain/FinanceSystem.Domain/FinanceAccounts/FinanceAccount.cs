using FinanceSystem.Domain.AccountEntries;
using FinanceSystem.Domain.AccountEntries.Enums;
using FinanceSystem.Domain.FinanceAccounts.Enums;
using FinanceSystem.Domain.FinanceAccounts.Exceptions;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.FinanceAccounts;

public class FinanceAccount : EntityBase<long>
{
    public FinanceAccountType Type { get; private set; }
    public FinanceAccountStatus Status { get; private set; }
    public Guid UserId { get; private set; }
    public decimal CachedBalance { get; private set; }
    public DateTimeOffset BalanceCalculatedAt { get; private set; }
    public bool AllowNegativeBalance { get; private set; }

    private FinanceAccount()
    {
    }

    public FinanceAccount(
        FinanceAccountType type,
        FinanceAccountStatus status,
        Guid userId,
        Money cachedBalance,
        DateTimeOffset balanceCalculatedAt,
        bool allowNegativeBalance)
    {
        Guard<InvalidIdException>.IsTrue(userId == Guid.Empty);
        Guard<NullEntryException>.AgainstNull(cachedBalance);

        Type = type;
        Status = status;
        UserId = userId;
        CachedBalance = cachedBalance.Value;
        BalanceCalculatedAt = balanceCalculatedAt;
        AllowNegativeBalance = allowNegativeBalance;
    }

    public void ApplyEntry(AccountEntry entry, DateTimeOffset occurredAt)
    {
        Guard<NullEntryException>.AgainstNull(entry);
        Guard<EntryFinanceAccountMismatchException>.IsTrue(entry.FinanceAccountId != Id);
        Guard<FinanceAccountClosedException>.IsTrue(Status == FinanceAccountStatus.Closed);
        Guard<FinanceAccountDirectionNotAllowedException>.IsFalse(Allows(entry.Direction));

        var newBalance = CachedBalance + entry.SignedAmount;

        Guard<InsufficientBalanceException>.IsTrue(newBalance < 0 && !AllowNegativeBalance);

        CachedBalance = newBalance;
        BalanceCalculatedAt = occurredAt;
    }

    private bool Allows(EntryDirection direction) => Status switch
    {
        FinanceAccountStatus.Active => true,
        FinanceAccountStatus.InboundFrozen => direction == EntryDirection.Out,
        FinanceAccountStatus.OutboundFrozen => direction == EntryDirection.In,
        FinanceAccountStatus.CompletelyFrozen => false,
        FinanceAccountStatus.Closed => false,
        _ => false
    };
}
