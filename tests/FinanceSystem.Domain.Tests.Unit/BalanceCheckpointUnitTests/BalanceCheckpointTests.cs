using FinanceSystem.Domain.BalanceCheckpoints.Exceptions;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using FluentAssertions;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.Tests.Unit.BalanceCheckpointUnitTests;

public class BalanceCheckpointTests
{
    private readonly BalanceCheckpointBuilder _builder;

    public BalanceCheckpointTests()
    {
        _builder = new BalanceCheckpointBuilder();
    }

    [Fact]
    public async Task Create_should_properly_create_balanceCheckpoint()
    {
        var bc = await _builder.Build();

        bc.AccountId.Should().Be(BalanceCheckpointBuilder.DefaultAccountId);
        bc.UpToEntryId.Should().Be(BalanceCheckpointBuilder.DefaultUpToEntryId);
        bc.BalanceRial.Should().Be(BalanceCheckpointBuilder.DefaultBalanceRial);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Create_should_throw_when_accountId_is_not_positive(long accountId)
    {
        Func<Task> bc = () => _builder.WithAccountId(accountId).Build();

        await bc.Should().ThrowAsync<InvalidAccountIdException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Create_should_throw_when_upToEntryId_is_not_positive(long upToEntryId)
    {
        Func<Task> bc = () => _builder.WithUpToEntryId(upToEntryId).Build();

        await bc.Should().ThrowAsync<InvalidUpToEntryIdException>();
    }

    [Fact]
    public async Task Create_should_throw_when_balance_is_null()
    {
        Func<Task> bc = () => _builder.WithBalanceAmount(null!).Build();

        await bc.Should().ThrowAsync<NullEntryException>();
    }

    [Fact]
    public async Task Create_should_throw_when_balance_currency_is_not_Rial()
    {
        Func<Task> bc = () => _builder
            .WithBalanceAmount(new Money(BalanceCheckpointBuilder.DefaultBalanceRial, Currency.Toman))
            .Build();

        await bc.Should().ThrowAsync<InvalidMoneyCurrencyException>();
    }
}
