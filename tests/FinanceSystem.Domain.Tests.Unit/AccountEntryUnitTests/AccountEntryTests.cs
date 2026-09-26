using FinanceSystem.Domain.AccountEntries.Enums;
using FinanceSystem.Domain.AccountEntries.Exceptions;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using FluentAssertions;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.Tests.Unit.AccountEntryUnitTests;

public class AccountEntryTests
{
    private readonly AccountEntryBuilder _builder;

    public AccountEntryTests()
    {
        _builder = new AccountEntryBuilder();
    }

    [Fact]
    public void Create_should_properly_create_accountEntry()
    {
        var accountEntry = _builder.Build();

        accountEntry.PaymentId.Should().Be(AccountEntryBuilder.DefaultPaymentId);
        accountEntry.FinanceAccountId.Should().Be(AccountEntryBuilder.DefaultFinanceAccountId);
        accountEntry.Amount.Should().Be(AccountEntryBuilder.DefaultAmount);
        accountEntry.Direction.Should().Be(AccountEntryBuilder.DefaultDirection);
    }

    [Theory]
    [InlineData(EntryDirection.In, 110)]
    [InlineData(EntryDirection.Out, -110)]
    public void SignedAmount_should_be_negative_only_for_outgoing_entries(
        EntryDirection direction,
        decimal expected)
    {
        var accountEntry = _builder.WithDirection(direction).Build();

        accountEntry.SignedAmount.Should().Be(expected);
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

        act.Should().Throw<InvalidAmountException>();
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
