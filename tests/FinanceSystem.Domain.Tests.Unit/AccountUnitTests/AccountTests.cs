using FinanceSystem.Domain.Accounts.Enums;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using FluentAssertions;
using NSubstitute.ExceptionExtensions;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.Tests.Unit.AccountUnitTests;

public class AccountTests
{
    private readonly AccountBuilder _accountBuilder;

    public AccountTests()
    {
        _accountBuilder = new AccountBuilder();
    }

    [Fact]
    public void Constructor_Should_Create_Properly_Account()
    {
        var account = _accountBuilder.Build();

        account.Type.Should().Be(AccountBuilder.DefaultType);
        account.Status.Should().Be(AccountBuilder.DefaultStatus);
        account.UserId.Should().Be(AccountBuilder.DefaultOwnerId);
        account.CachedBalance.Should().Be(AccountBuilder.DefaultCachedBalance);
        account.BalanceCalculatedAt.Should().Be(AccountBuilder.DefaultBalanceCalculatedAt);
        account.AllowNegativeBalance.Should().Be(AccountBuilder.DefaultAllowNegativeBalance);
    }

    [Fact]
    public void Create_should_throw_when_ownerId_is_empty_guid()
    {
        Action act = () => _accountBuilder.WithOwnerId(Guid.Empty).Build();

        act.Should().Throw<InvalidIdException>();
    }

    [Fact]
    public void Create_should_throw_when_cachedBalance_is_null()
    {
        Action account = () => _accountBuilder.WithAmount(null!).Build();

        account.Should().Throw<NullEntryException>();
    }
}
