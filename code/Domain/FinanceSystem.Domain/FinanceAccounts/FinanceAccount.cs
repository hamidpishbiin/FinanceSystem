using FinanceSystem.Domain.LedgerEntries;
using FinanceSystem.Domain.LedgerEntries.Enums;
using FinanceSystem.Domain.AccountTypeRules;
using FinanceSystem.Domain.FinanceAccounts.Enums;
using FinanceSystem.Domain.FinanceAccounts.Exceptions;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.FinanceAccounts;

public class FinanceAccount : EntityBase<long>
{
    public FinanceAccountType Type { get; private set; }
    public FinanceAccountState State { get; private set; }
    public long UserId { get; private set; }
    public decimal CachedBalance { get; private set; }
    public DateTimeOffset BalanceCalculatedAt { get; private set; }
    public bool AllowNegativeBalance { get; private set; }

    private FinanceAccount()
    {
    }

    public FinanceAccount(
        FinanceAccountType type,
        FinanceAccountState state,
        long userId,
        Money cachedBalance,
        DateTimeOffset balanceCalculatedAt,
        bool allowNegativeBalance)
    {
        Guard<InvalidIdException>.IsTrue(userId <= 0);
        Guard<NullEntryException>.AgainstNull(cachedBalance);

        Type = type;
        State = state;
        UserId = userId;
        CachedBalance = cachedBalance.Value;
        BalanceCalculatedAt = balanceCalculatedAt;
        AllowNegativeBalance = allowNegativeBalance;
    }

    public void ApplyEntry(LedgerEntry entry, DateTimeOffset occurredAt)
    {
        Guard<NullEntryException>.AgainstNull(entry);
        Guard<EntryFinanceAccountMismatchException>.IsTrue(entry.FinanceAccountId != Id);
        Guard<FinanceAccountDirectionNotAllowedException>.IsFalse(Allows(entry.Direction));

        var newBalance = CachedBalance + entry.SignedAmount;

        Guard<InsufficientBalanceException>.IsTrue(newBalance < 0 && !AllowNegativeBalance);

        CachedBalance = newBalance;
        BalanceCalculatedAt = occurredAt;
    }

    public bool IsAllowedToPurchase(List<AccountTypeRule> rules)
    {
        return IsTransactionAllowed(rules, PaymentPurpose.Purchase);
    }

    public bool IsTopUpAllowed(List<AccountTypeRule> rules)
    {
        return IsTransactionAllowed(rules, PaymentPurpose.TopUp);
    }

    private bool IsTransactionAllowed(List<AccountTypeRule> rules, PaymentPurpose purpose)
    {
        return rules.FirstOrDefault(ar => ar.Type == Type && ar.State == State && ar.Purpose == purpose)?.IsAllowed ?? false;
    }

    private bool Allows(EntryDirection direction) => State switch
    {
        FinanceAccountState.Active => true,
        FinanceAccountState.InboundFrozen => direction == EntryDirection.Outbound,
        FinanceAccountState.OutboundFrozen => direction == EntryDirection.Inbound,
        FinanceAccountState.CompletelyFrozen => false,
        _ => false
    };
}
