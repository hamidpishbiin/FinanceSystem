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
    public long OwnerId { get; private set; }
    public decimal CachedBalanceRial { get; private set; }
    public DateTimeOffset BalanceCalculatedAt { get; private set; }
    public bool AllowNegativeBalance { get; private set; }

    private Account()
    {
    }

    public static Account Create(
        AccountType type,
        AccountStatus status,
        long ownerId,
        Money cachedBalanceRial,
        DateTimeOffset balanceCalculatedAt,
        bool allowNegativeBalance)
    {
        Guard<InvalidIdException>.IsTrue(ownerId <= 0);
        Guard<NullEntryException>.AgainstNull(cachedBalanceRial);
        Guard<InvalidMoneyCurrencyException>.IsTrue(cachedBalanceRial.Currency != Currency.Rial);

        return new Account()
        {
            Type = type,
            Status = status,
            OwnerId = ownerId,
            CachedBalanceRial = cachedBalanceRial.Value,
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

        var newBalance = CachedBalanceRial + entry.SignedAmountRial;

        Guard<InsufficientBalanceException>.IsTrue(newBalance < 0 && !AllowNegativeBalance);

        CachedBalanceRial = newBalance;
        BalanceCalculatedAt = occurredAt;
    }

    public bool Allows(EntryDirection direction) => Status switch
    {
        AccountStatus.Active => true,
        AccountStatus.InboundFrozen => direction == EntryDirection.Out,
        AccountStatus.OutboundFrozen => direction == EntryDirection.In,
        AccountStatus.CompletelyFrozen => false,
        AccountStatus.Closed => false,
        _ => false
    };
}
