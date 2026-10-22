using FinanceSystem.Domain.FinanceAccounts.Exceptions;
using FinanceSystem.Domain.LedgerEntries.Enums;
using FinanceSystem.Domain.LedgerEntries.Exceptions;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using FluentAssertions;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.Tests.Unit.LedgerEntryUnitTests;

public class LedgerEntryTests
{
    private readonly LedgerEntryBuilder _builder;

    public LedgerEntryTests()
    {
        _builder = new LedgerEntryBuilder();
    }

    [Fact]
    public void Create_should_properly_create_ledgerEntry()
    {
        var ledgerEntry = _builder.Build();

        ledgerEntry.PaymentId.Should().Be(LedgerEntryBuilder.DefaultPaymentId);
        ledgerEntry.FinanceAccountId.Should().Be(LedgerEntryBuilder.DefaultFinanceAccountId);
        ledgerEntry.Amount.Should().Be(LedgerEntryBuilder.DefaultAmount);
        ledgerEntry.Direction.Should().Be(LedgerEntryBuilder.DefaultDirection);
    }

    [Theory]
    [InlineData(EntryDirection.In, 110)]
    [InlineData(EntryDirection.Out, -110)]
    public void SignedAmount_should_be_negative_only_for_outgoing_entries(
        EntryDirection direction,
        decimal expected)
    {
        var ledgerEntry = _builder.WithDirection(direction).Build();

        ledgerEntry.SignedAmount.Should().Be(expected);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_should_throw_when_PaymentId_is_not_positive(long paymentId)
    {
        Action act = () => _builder.WithPaymentId(paymentId).Build();

        act.Should().Throw<InvalidPaymentIdException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_should_throw_when_FinanceAccountId_is_not_positive(long financeAccountId)
    {
        Action act = () => _builder.WithFinanceAccountId(financeAccountId).Build();

        act.Should().Throw<InvalidFinanceAccountIdException>();
    }

    [Fact]
    public void Create_should_throw_when_Amount_is_null()
    {
        Action act = () => _builder.WithAmount(null!).Build();

        act.Should().Throw<NullEntryException>();
    }

    [Fact]
    public void Create_should_throw_when_Amount_is_zero()
    {
        Action act = () => _builder.WithAmount(new Money(0)).Build();

        act.Should().Throw<InvalidMoneyAmountException>();
    }

    [Theory]
    [InlineData((EntryDirection)0)]
    [InlineData((EntryDirection)99)]
    public void Create_should_throw_when_Direction_is_not_defined(EntryDirection direction)
    {
        Action act = () => _builder.WithDirection(direction).Build();

        act.Should().Throw<InvalidEntryDirectionException>();
    }
}
