using FinanceSystem.Domain.Payments;
using Shared.Domain.Exceptions;
using FluentAssertions;

namespace FinanceSystem.Domain.Tests.Unit.MoneyUnitTests;

public class MoneyTests
{
    [Fact]
    public void Constructor_should_properly_create_money()
    {
        var money = new Money(250_000);

        money.Value.Should().Be(250_000);
    }

    [Fact]
    public void Constructor_should_allow_zero_value()
    {
        var money = new Money(0);

        money.Value.Should().Be(0);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-0.01)]
    public void Constructor_should_throw_when_value_is_negative(decimal value)
    {
        Action act = () => _ = new Money(value);

        act.Should().Throw<InvalidMoneyAmountException>();
    }

    [Fact]
    public void Money_with_same_value_should_be_equal_but_not_the_same_instance()
    {
        var a = new Money(250_000);
        var b = new Money(250_000);

        a.Should().Be(b);
        a.Should().NotBeSameAs(b);
        (a == b).Should().BeTrue();
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void Money_with_different_value_should_not_be_equal()
    {
        new Money(100).Should().NotBe(new Money(200));
    }

    [Fact]
    public void ToString_should_render_value_in_rial()
    {
        new Money(250_000).ToString().Should().Be("250000 Rial");
    }
}
