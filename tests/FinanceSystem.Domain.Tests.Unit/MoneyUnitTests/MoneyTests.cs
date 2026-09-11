using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using FluentAssertions;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.Tests.Unit.MoneyUnitTests;

public class MoneyTests
{
    [Fact]
    public void Constructor_should_properly_create_money()
    {
        var money = new Money(250_000, Currency.Rial);

        money.Value.Should().Be(250_000);
        money.Currency.Should().Be(Currency.Rial);
    }

    [Fact]
    public void Constructor_should_allow_zero_value()
    {
        var money = new Money(0, Currency.Rial);

        money.Value.Should().Be(0);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-0.01)]
    public void Constructor_should_throw_when_value_is_negative(decimal value)
    {
        Action act = () => _ = new Money(value, Currency.Rial);

        act.Should().Throw<InvalidMoneyAmountException>();
    }

    [Theory]
    [InlineData((Currency)2)]
    [InlineData((Currency)99)]
    public void Constructor_should_throw_when_currency_is_not_defined(Currency currency)
    {
        Action act = () => _ = new Money(100, currency);

        act.Should().Throw<InvalidMoneyCurrencyException>();
    }

    [Fact]
    public void Money_with_same_value_and_currency_should_be_equal_but_not_the_same_instance()
    {
        var a = new Money(250_000, Currency.Rial);
        var b = new Money(250_000, Currency.Rial);

        a.Should().Be(b);
        a.Should().NotBeSameAs(b);
        (a == b).Should().BeTrue();
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void Money_with_same_value_but_different_currency_should_not_be_equal()
    {
        var rial = new Money(100, Currency.Rial);
        var toman = new Money(100, Currency.Toman);

        rial.Should().NotBe(toman);
    }

    [Fact]
    public void ToString_should_render_value_and_currency()
    {
        new Money(250_000, Currency.Rial).ToString().Should().Be("250000 Rial");
    }
}
