using FinanceSystem.Domain.FinanceAccounts;
using FinanceSystem.Domain.FinanceAccounts.Enums;
using FinanceSystem.Domain.FinanceAccounts.Exceptions;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using FluentAssertions;
using NSubstitute.ExceptionExtensions;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.Tests.Unit.FinanceAccountUnitTests;

public class FinanceAccountTests
{
    private readonly FinanceAccountBuilder _financeAccountBuilder;

    public FinanceAccountTests()
    {
        _financeAccountBuilder = new FinanceAccountBuilder();
    }

    [Fact]
    public void Constructor_Should_Create_Properly_FinanceAccount()
    {
        var financeAccount = _financeAccountBuilder.Build();

        financeAccount.Type.Should().Be(FinanceAccountBuilder.DefaultType);
        financeAccount.Status.Should().Be(FinanceAccountBuilder.DefaultStatus);
        financeAccount.UserId.Should().Be(FinanceAccountBuilder.DefaultUserId);
        financeAccount.CachedBalance.Should().Be(FinanceAccountBuilder.DefaultCachedBalance);
        financeAccount.BalanceCalculatedAt.Should().Be(FinanceAccountBuilder.DefaultBalanceCalculatedAt);
        financeAccount.AllowNegativeBalance.Should().Be(FinanceAccountBuilder.DefaultAllowNegativeBalance);
    }

    [Fact]
    public void Create_should_throw_when_userId_is_empty_guid()
    {
        Action act = () => _financeAccountBuilder.WithUserId(Guid.Empty).Build();

        act.Should().Throw<InvalidIdException>();
    }

    [Fact]
    public void Create_should_throw_when_cachedBalance_is_null()
    {
        Action financeAccount = () => _financeAccountBuilder.WithAmount(null!).Build();

        financeAccount.Should().Throw<NullEntryException>();
    }

    [Fact]
    public void Create_should_succeed_when_company_wallet_is_owned_by_company()
    {
        var financeAccount = _financeAccountBuilder
            .WithType(FinanceAccountType.CompanyWallet)
            .WithUserId(SystemFinanceAccounts.CompanyUserId)
            .Build();

        financeAccount.Type.Should().Be(FinanceAccountType.CompanyWallet);
        financeAccount.UserId.Should().Be(SystemFinanceAccounts.CompanyUserId);
    }

    [Fact]
    public void Create_should_throw_when_company_wallet_is_not_owned_by_company()
    {
        Action act = () => _financeAccountBuilder.WithType(FinanceAccountType.CompanyWallet).Build();

        act.Should().Throw<SystemFinanceAccountOwnerMismatchException>();
    }

    [Theory]
    [InlineData(FinanceAccountType.UserWallet)]
    [InlineData(FinanceAccountType.CreditWallet)]
    public void Create_should_throw_when_user_type_is_owned_by_company(FinanceAccountType type)
    {
        Action act = () => _financeAccountBuilder
            .WithType(type)
            .WithUserId(SystemFinanceAccounts.CompanyUserId)
            .Build();

        act.Should().Throw<SystemFinanceAccountOwnerMismatchException>();
    }
}
