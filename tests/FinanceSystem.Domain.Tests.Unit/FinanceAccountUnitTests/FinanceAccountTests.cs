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
        financeAccount.State.Should().Be(FinanceAccountBuilder.DefaultState);
        financeAccount.UserId.Should().Be(FinanceAccountBuilder.DefaultUserId);
        financeAccount.CachedBalance.Should().Be(FinanceAccountBuilder.DefaultCachedBalance);
        financeAccount.BalanceCalculatedAt.Should().Be(FinanceAccountBuilder.DefaultBalanceCalculatedAt);
        financeAccount.AllowNegativeBalance.Should().Be(FinanceAccountBuilder.DefaultAllowNegativeBalance);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_should_throw_when_userId_is_not_positive(long userId)
    {
        Action act = () => _financeAccountBuilder.WithUserId(userId).Build();

        act.Should().Throw<InvalidIdException>();
    }

    [Fact]
    public void Create_should_throw_when_cachedBalance_is_null()
    {
        Action financeAccount = () => _financeAccountBuilder.WithAmount(null!).Build();

        financeAccount.Should().Throw<NullEntryException>();
    }
}
