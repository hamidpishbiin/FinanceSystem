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
    public void Create_should_properly_create_balanceCheckpoint()
    {
        var bc = _builder.Build();

        bc.FinanceAccountId.Should().Be(BalanceCheckpointBuilder.DefaultFinanceAccountId);
        bc.UpToEntryId.Should().Be(BalanceCheckpointBuilder.DefaultUpToEntryId);
        bc.Balance.Should().Be(BalanceCheckpointBuilder.DefaultBalance);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_should_throw_when_financeAccountId_is_not_positive(long financeAccountId)
    {
        Action bc = () => _builder.WithFinanceAccountId(financeAccountId).Build();

        bc.Should().Throw<InvalidFinanceAccountIdException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_should_throw_when_upToEntryId_is_not_positive(long upToEntryId)
    {
        Action bc = () => _builder.WithUpToEntryId(upToEntryId).Build();

        bc.Should().Throw<InvalidUpToEntryIdException>();
    }

    [Fact]
    public void Create_should_throw_when_balance_is_null()
    {
        Action bc = () => _builder.WithBalanceAmount(null!).Build();

        bc.Should().Throw<NullEntryException>();
    }
}
