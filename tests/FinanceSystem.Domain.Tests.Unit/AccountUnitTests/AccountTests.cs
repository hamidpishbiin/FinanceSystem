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
    public async Task Constructor_Should_Create_Properly_Account()
    {
        var account = await _accountBuilder.Build();

        account.Type.Should().Be(AccountBuilder.DefaultType);
        account.Status.Should().Be(AccountBuilder.DefaultStatus);
        account.OwnerId.Should().Be(AccountBuilder.DefaultOwnerId);
        account.CachedBalanceRial.Should().Be(AccountBuilder.DefaultCachedBalanceRial);
        account.BalanceCalculatedAt.Should().Be(AccountBuilder.DefaultBalanceCalculatedAt);
        account.AllowNegativeBalance.Should().Be(AccountBuilder.DefaultAllowNegativeBalance);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Create_should_throw_when_ownerId_is_not_positive(long ownerId)
    {
        Func<Task> act = async () => await _accountBuilder.WithOwnerId(ownerId).Build();

        await act.Should().ThrowAsync<InvalidIdException>();
    }

    [Fact]
    public async Task Create_should_throw_when_cachedBalanceRial_is_null()
    {
        Func<Task> account = () => _accountBuilder.WithAmount(null!).Build();

        await account.Should().ThrowAsync<NullEntryException>();
    }

    [Fact]
    public async Task Create_should_throw_when_currency_is_not_rial()
    {
        Func<Task> account = () => _accountBuilder
            .WithAmount(new Money(AccountBuilder.DefaultCachedBalanceRial, Currency.Toman))
            .Build();

        await account.Should().ThrowAsync<InvalidMoneyCurrencyException>();
    }
}
