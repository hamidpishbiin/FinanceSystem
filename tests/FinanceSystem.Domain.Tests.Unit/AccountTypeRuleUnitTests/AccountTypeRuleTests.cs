using FinanceSystem.Domain.Accounts.Enums;
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
    public async Task Create_should_properly_create_accountTypeRule()
    {
        var atr = await _builder
            .WithCanBeSource(true)
            .WithCanBeDestination(false)
            .Build();

        atr.Type.Should().Be(AccountTypeRuleBuilder.DefaultType);
        atr.Purpose.Should().Be(AccountTypeRuleBuilder.DefaultPaymentPurpose);
        atr.CanBeSource.Should().BeTrue();
        atr.CanBeDestination.Should().BeFalse();
    }

    [Theory]
    [InlineData((AccountType)0)]
    [InlineData((AccountType)10)]
    public async Task Create_should_throw_when_accountType_is_not_defined(AccountType accountType)
    {
        Func<Task> atr = () => _builder.WithType(accountType).Build();

        await atr.Should().ThrowAsync<InvalidAccountTypeException>();
    }

    [Theory]
    [InlineData((PaymentPurpose)0)]
    [InlineData((PaymentPurpose)10)]
    public async Task Create_should_throw_when_paymentPurpose_is_not_defined(PaymentPurpose paymentPurpose)
    {
        Func<Task> atr = () => _builder.WithPurpose(paymentPurpose).Build();

        await atr.Should().ThrowAsync<InvalidPaymentPurposeException>();
    }

    [Fact]
    public async Task Create_should_throw_when_both_canBeSource_and_canBeDestination_are_False()
    {
        Func<Task> atr = () => _builder
            .WithCanBeSource(false)
            .WithCanBeDestination(false)
            .Build();

        await atr.Should().ThrowAsync<InvalidSourceDestinationException>();
    }
}
