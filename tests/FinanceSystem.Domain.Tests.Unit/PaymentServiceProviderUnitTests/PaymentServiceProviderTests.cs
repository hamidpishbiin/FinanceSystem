using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Exceptions;
using FluentAssertions;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.Tests.Unit.PaymentServiceProviderUnitTests;

public class PaymentServiceProviderTests
{
    private readonly PaymentServiceProviderBuilder _builder = new();

    [Fact]
    public void Create_should_properly_create_paymentServiceProvider()
    {
        var psp = _builder.Build();

        psp.Id.Should().Be(PaymentServiceProviderBuilder.DefaultId);
        psp.Code.Should().Be(PaymentServiceProviderBuilder.DefaultCode);
        psp.Name.Should().Be(PaymentServiceProviderBuilder.DefaultName);
        psp.IsActive.Should().Be(PaymentServiceProviderBuilder.DefaultIsActive);
        psp.Priority.Should().Be(PaymentServiceProviderBuilder.DefaultPriority);
    }

    [Fact]
    public void Create_should_throw_when_id_is_empty_guid()
    {
        Action act = () => _builder.WithId(Guid.Empty).Build();

        act.Should().Throw<InvalidIdException>();
    }

    [Theory]
    [InlineData((PspCode)0)]
    [InlineData((PspCode)99)]
    public void Create_should_throw_when_code_is_not_defined(PspCode code)
    {
        Action act = () => _builder.WithCode(code).Build();

        act.Should().Throw<InvalidPspCodeException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_should_throw_when_name_is_null_or_empty_or_whiteSpace(string? name)
    {
        Action act = () => _builder.WithName(name).Build();

        act.Should().Throw<InvalidPspNameException>();
    }

    [Theory]
    [InlineData((short)-1)]
    [InlineData(short.MinValue)]
    public void Create_should_throw_when_priority_is_negative(short priority)
    {
        Action act = () => _builder.WithPriority(priority).Build();

        act.Should().Throw<InvalidPriorityException>();
    }

    [Fact]
    public void Create_should_allow_zero_priority()
    {
        var psp = _builder.WithPriority(0).Build();

        psp.Priority.Should().Be(0);
    }

    [Fact]
    public void Create_should_allow_inactive_provider()
    {
        var psp = _builder.WithIsActive(false).Build();

        psp.IsActive.Should().BeFalse();
    }

    [Fact]
    public void PsPCode_values_are_persisted_contract_and_should_not_change()
    {
        ((int)PspCode.Saman).Should().Be(1);
        ((int)PspCode.BehPardakht).Should().Be(2);

        Enum.GetValues<PspCode>().Should().HaveCount(2,
            "adding a provider requires a matching row in PaymentServiceProviders and any Code check constraint");
    }
}
