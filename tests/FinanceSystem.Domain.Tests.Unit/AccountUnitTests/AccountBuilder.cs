using FinanceSystem.Domain.Accounts;
using FinanceSystem.Domain.Accounts.Enums;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;

namespace FinanceSystem.Domain.Tests.Unit.AccountUnitTests;

public class AccountBuilder
{
    public const AccountType DefaultType = AccountType.BankGateway;
    public const AccountStatus DefaultStatus = AccountStatus.Active;
    public const long DefaultOwnerId = 1234;
    public const decimal DefaultCachedBalanceRial = 300;
    public static readonly DateTimeOffset DefaultBalanceCalculatedAt = new(2026, 08, 09, 0, 0, 0, TimeSpan.Zero);
    public const bool DefaultAllowNegativeBalance = false;

    private AccountType Type { get; set; } = DefaultType;
    private AccountStatus Status { get; set; } = DefaultStatus;
    private long OwnerId { get; set; } = DefaultOwnerId;
    private Money CachedAmount { get; set; } = new(DefaultCachedBalanceRial, Currency.Rial);
    private DateTimeOffset BalanceCalculatedAt { get; set; } = DefaultBalanceCalculatedAt;
    private bool AllowNegativeBalance { get; set; } = DefaultAllowNegativeBalance;

    public async Task<Account> Build()
    {
        return await Account.Create(
            Type,
            Status,
            OwnerId,
            CachedAmount,
            BalanceCalculatedAt,
            AllowNegativeBalance);
    }

    public AccountBuilder WithType(AccountType type)
    {
        Type = type;
        return this;
    }

    public AccountBuilder WithStatus(AccountStatus status)
    {
        Status = status;
        return this;
    }

    public AccountBuilder WithOwnerId(long ownerId)
    {
        OwnerId = ownerId;
        return this;
    }

    public AccountBuilder WithAmount(Money cachedBalanceRial)
    {
        CachedAmount = cachedBalanceRial;
        return this;
    }

    public AccountBuilder WithBalanceCalculatedAt(DateTimeOffset balanceCalculatedAt)
    {
        BalanceCalculatedAt = balanceCalculatedAt;
        return this;
    }

    public AccountBuilder WithAllowNegativeBalance(bool allowNegativeBalance)
    {
        AllowNegativeBalance = allowNegativeBalance;
        return this;
    }
}
