using FinanceSystem.Domain.Accounts;
using FinanceSystem.Domain.Accounts.Enums;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;

namespace FinanceSystem.Domain.Tests.Unit.AccountUnitTests;

public class AccountBuilder
{
    public const AccountType DefaultType = AccountType.UserWallet;
    public const AccountStatus DefaultStatus = AccountStatus.Active;
    public const string DefaultOwnerIdString = "3c9a7e21-5b4d-4f8e-9a1c-2d6e8f0b7a45";
    public const decimal DefaultCachedBalance = 300;
    public static readonly DateTimeOffset DefaultBalanceCalculatedAt = new(2026, 08, 09, 0, 0, 0, TimeSpan.Zero);
    public const bool DefaultAllowNegativeBalance = false;

    public static readonly Guid DefaultOwnerId = Guid.Parse(DefaultOwnerIdString);

    private AccountType Type { get; set; } = DefaultType;
    private AccountStatus Status { get; set; } = DefaultStatus;
    private Guid OwnerId { get; set; } = DefaultOwnerId;
    private Money CachedAmount { get; set; } = new(DefaultCachedBalance);
    private DateTimeOffset BalanceCalculatedAt { get; set; } = DefaultBalanceCalculatedAt;
    private bool AllowNegativeBalance { get; set; } = DefaultAllowNegativeBalance;

    public Account Build()
    {
        return new Account(
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

    public AccountBuilder WithOwnerId(Guid ownerId)
    {
        OwnerId = ownerId;
        return this;
    }

    public AccountBuilder WithAmount(Money cachedBalance)
    {
        CachedAmount = cachedBalance;
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
