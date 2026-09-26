using FinanceSystem.Domain.FinanceAccounts.Enums;
using FinanceSystem.Domain.AccountTypeRules.Exceptions;
using FinanceSystem.Domain.Payments.Enums;
using FluentAssertions;

namespace FinanceSystem.Domain.Tests.Unit.AccountTypeRuleUnitTests;

public class AccountTypeRuleTests
{
    private readonly AccountTypeRuleBuilder _builder;

    public AccountTypeRuleTests()
    {
        _builder = new AccountTypeRuleBuilder();
    }

    [Fact]
    public void Create_should_properly_create_accountTypeRule()
    {
        var atr = _builder
            .WithCanBeSource(true)
            .WithCanBeDestination(false)
            .Build();

        atr.Type.Should().Be(AccountTypeRuleBuilder.DefaultType);
        atr.Purpose.Should().Be(AccountTypeRuleBuilder.DefaultPaymentPurpose);
        atr.CanBeSource.Should().BeTrue();
        atr.CanBeDestination.Should().BeFalse();
    }

    [Theory]
    [InlineData((FinanceAccountType)0)]
    [InlineData((FinanceAccountType)10)]
    public void Create_should_throw_when_financeAccountType_is_not_defined(FinanceAccountType financeAccountType)
    {
        Action atr = () => _builder.WithType(financeAccountType).Build();

        atr.Should().Throw<InvalidFinanceAccountTypeException>();
    }

    [Theory]
    [InlineData((PaymentPurpose)0)]
    [InlineData((PaymentPurpose)10)]
    public void Create_should_throw_when_paymentPurpose_is_not_defined(PaymentPurpose paymentPurpose)
    {
        Action atr = () => _builder.WithPurpose(paymentPurpose).Build();

        atr.Should().Throw<InvalidPaymentPurposeException>();
    }

    [Fact]
    public void Create_should_throw_when_both_canBeSource_and_canBeDestination_are_False()
    {
        Action atr = () => _builder
            .WithCanBeSource(false)
            .WithCanBeDestination(false)
            .Build();

        atr.Should().Throw<InvalidSourceDestinationException>();
    }
}
