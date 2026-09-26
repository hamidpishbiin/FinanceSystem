using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Exceptions;
using FinanceSystem.Domain.RequestsToPay.Enums;
using FinanceSystem.Domain.RequestsToPay.Exceptions;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using FluentAssertions;
using NSubstitute;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.Tests.Unit.RequestToPayUnitTests;

public class RequestToPayTests
{
    private readonly RequestToPayBuilder _builder = new();

    [Fact]
    public void Create_should_properly_create_requestToPay()
    {
        var bpd = _builder.Build();

        bpd.PspCode.Should().Be(RequestToPayBuilder.DefaultPspCode);
        bpd.Status.Should().Be(RequestToPayStatus.Initiated);
        bpd.RequestAmount.Should().Be(RequestToPayBuilder.DefaultRequestAmount);
        bpd.RedirectedAmount.Should().BeNull();
        bpd.TargetFinanceAccountId.Should().Be(RequestToPayBuilder.DefaultTargetFinanceAccountId);
        bpd.Token.Should().BeNull();
        bpd.RRN.Should().BeNull();
        bpd.RefNum.Should().BeNull();
        bpd.ReferenceNumber.Should().Be(0, "the database sequence assigns it on insert");
        bpd.TraceNumber.Should().BeNull();
        bpd.MaskedPan.Should().BeNull();
        bpd.FailureReason.Should().BeNull();
        bpd.RawStatus.Should().BeNull();
        bpd.RawErrorCode.Should().BeNull();
        bpd.RawDescription.Should().BeNull();
        bpd.VerifiedAtUtc.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(99)]
    public void Create_should_throw_when_pspCode_is_not_defined(int pspCode)
    {
        Action bpd = () => _builder.WithPspCode((PspCode)pspCode).Build();

        bpd.Should().Throw<InvalidPspCodeException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_should_throw_when_financeAccountId_is_not_positive(long financeAccountId)
    {
        Action bpd = () => _builder.WithTargetFinanceAccountId(financeAccountId).Build();

        bpd.Should().Throw<InvalidTargetFinanceAccountIdException>();
    }

    [Fact]
    public void Create_should_throw_when_requestAmount_is_null()
    {
        Action bpd = () => _builder.WithRequestAmount(null!).Build();

        bpd.Should().Throw<NullEntryException>();
    }

    [Fact]
    public void Create_should_throw_when_requestAmount_is_zero()
    {
        Action bpd = () => _builder.WithRequestAmount(new Money(0)).Build();

        bpd.Should().Throw<InvalidRequestAmountException>();
    }


    [Fact]
    public void Create_should_throw_when_eventPublisher_is_null()
    {
        Action bpd = () => _builder.WithEventPublisher(null).Build();

        bpd.Should().Throw<NullEntryException>();
    }

    [Fact]
    public void RequestToPayStatus_values_are_persisted_contract_and_should_not_change()
    {
        ((int)RequestToPayStatus.Initiated).Should().Be(1);
        ((int)RequestToPayStatus.TokenReceived).Should().Be(2);
        ((int)RequestToPayStatus.CallbackReceived).Should().Be(3);
        ((int)RequestToPayStatus.Verified).Should().Be(4);
        ((int)RequestToPayStatus.Failed).Should().Be(5);
        ((int)RequestToPayStatus.Expired).Should().Be(6);
        ((int)RequestToPayStatus.Reversed).Should().Be(7);

        Enum.GetValues<RequestToPayStatus>().Should().HaveCount(7,
            "adding a member requires updating the Status check constraint and any DDL");
    }
}
