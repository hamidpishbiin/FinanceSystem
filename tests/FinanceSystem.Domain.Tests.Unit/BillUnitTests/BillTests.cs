using FinanceSystem.Domain.FinanceAccounts.Exceptions;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Exceptions;
using FluentAssertions;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.Tests.Unit.BillUnitTests;

public class BillTests
{
    private readonly BillBuilder _builder = new();

    [Fact]
    public void Constructor_should_create_bill_properly()
    {
        var bill = _builder.Build();

        bill.ServiceId.Should().Be(BillBuilder.DefaultServiceId);
        bill.ReferenceId.Should().Be(BillBuilder.DefaultReferenceId);
        bill.FinanceAccountId.Should().Be(BillBuilder.DefaultFinanceAccountId);
        bill.Amount.Should().Be(BillBuilder.DefaultAmount);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_should_throw_if_serviceId_is_zero_or_negative(long serviceId)
    {
        Action act = () => _builder.WithServiceId(serviceId).Build();

        act.Should().Throw<InvalidServiceIdException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_should_throw_if_referenceId_is_zero_or_negative(long referenceId)
    {
        Action act = () => _builder.WithReferenceId(referenceId).Build();

        act.Should().Throw<InvalidReferenceIdException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_should_throw_if_financeAccountId_is_zero_or_negative(long financeAccountId)
    {
        Action act = () => _builder.WithFinanceAccountId(financeAccountId).Build();

        act.Should().Throw<InvalidFinanceAccountIdException>();
    }

    [Fact]
    public void Create_should_throw_if_amount_is_zero()
    {
        Action act = () => _builder.WithAmount(new Money(0)).Build();

        act.Should().Throw<InvalidMoneyAmountException>();
    }
}
