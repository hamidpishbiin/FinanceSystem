using FinanceSystem.Domain.FinanceAccounts;
using FinanceSystem.Domain.FinanceAccounts.Enums;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;

namespace FinanceSystem.Domain.Tests.Unit.FinanceAccountUnitTests;

public class FinanceAccountBuilder
{
    public const FinanceAccountType DefaultType = FinanceAccountType.UserWallet;
    public const FinanceAccountStatus DefaultStatus = FinanceAccountStatus.Active;
    public const string DefaultUserIdString = "3c9a7e21-5b4d-4f8e-9a1c-2d6e8f0b7a45";
    public const decimal DefaultCachedBalance = 300;
    public static readonly DateTimeOffset DefaultBalanceCalculatedAt = new(2026, 08, 09, 0, 0, 0, TimeSpan.Zero);
    public const bool DefaultAllowNegativeBalance = false;

    public static readonly Guid DefaultUserId = Guid.Parse(DefaultUserIdString);

    private FinanceAccountType Type { get; set; } = DefaultType;
    private FinanceAccountStatus Status { get; set; } = DefaultStatus;
    private Guid UserId { get; set; } = DefaultUserId;
    private Money CachedAmount { get; set; } = new(DefaultCachedBalance);
    private DateTimeOffset BalanceCalculatedAt { get; set; } = DefaultBalanceCalculatedAt;
    private bool AllowNegativeBalance { get; set; } = DefaultAllowNegativeBalance;

    public FinanceAccount Build()
    {
        return new FinanceAccount(
            Type,
            Status,
            UserId,
            CachedAmount,
            BalanceCalculatedAt,
            AllowNegativeBalance);
    }

    public FinanceAccountBuilder WithType(FinanceAccountType type)
    {
        Type = type;
        return this;
    }

    public FinanceAccountBuilder WithStatus(FinanceAccountStatus status)
    {
        Status = status;
        return this;
    }

    public FinanceAccountBuilder WithUserId(Guid userId)
    {
        UserId = userId;
        return this;
    }

    public FinanceAccountBuilder WithAmount(Money cachedBalance)
    {
        CachedAmount = cachedBalance;
        return this;
    }

    public FinanceAccountBuilder WithBalanceCalculatedAt(DateTimeOffset balanceCalculatedAt)
    {
        BalanceCalculatedAt = balanceCalculatedAt;
        return this;
    }

    public FinanceAccountBuilder WithAllowNegativeBalance(bool allowNegativeBalance)
    {
        AllowNegativeBalance = allowNegativeBalance;
        return this;
    }
}
