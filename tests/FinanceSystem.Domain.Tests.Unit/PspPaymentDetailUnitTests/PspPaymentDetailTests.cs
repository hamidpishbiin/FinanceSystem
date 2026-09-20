using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Exceptions;
using FinanceSystem.Domain.PspPaymentDetails.Enums;
using FinanceSystem.Domain.PspPaymentDetails.Exceptions;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using FluentAssertions;
using NSubstitute;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.Tests.Unit.PspPaymentDetailUnitTests;

public class PspPaymentDetailTests
{
    private readonly PspPaymentDetailBuilder _builder = new();

    [Fact]
    public async Task Create_should_properly_create_pspPaymentDetail()
    {
        var bpd = await _builder.Build();

        bpd.PspCode.Should().Be(PspPaymentDetailBuilder.DefaultPspCode);
        bpd.Status.Should().Be(PspPaymentStatus.Initiated);
        bpd.RequestAmountRial.Should().Be(PspPaymentDetailBuilder.DefaultRequestAmountRial);
        bpd.RedirectedAmountRial.Should().BeNull();
        bpd.TargetAccountId.Should().Be(PspPaymentDetailBuilder.DefaultTargetAccountId);
        bpd.Token.Should().BeNull();
        bpd.RRN.Should().BeNull();
        bpd.RefNum.Should().BeNull();
        bpd.ReferenceNumber.Should().Be(0, "the database sequence assigns it on insert");
        bpd.TraceNumber.Should().BeNull();
        bpd.MaskedPan.Should().BeNull();
        bpd.ResultCode.Should().BeNull();
        bpd.RawCallback.Should().BeNull();
        bpd.VerifiedAtUtc.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(99)]
    public async Task Create_should_throw_when_pspCode_is_not_defined(int pspCode)
    {
        Func<Task> bpd = () => _builder.WithPspCode((PspCode)pspCode).Build();

        await bpd.Should().ThrowAsync<InvalidPspCodeException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Create_should_throw_when_accountId_is_not_positive(long accountId)
    {
        Func<Task> bpd = () => _builder.WithTargetAccountId(accountId).Build();

        await bpd.Should().ThrowAsync<InvalidTargetAccountIdException>();
    }

    [Fact]
    public async Task Create_should_throw_when_requestAmount_is_null()
    {
        Func<Task> bpd = () => _builder.WithRequestAmount(null!).Build();

        await bpd.Should().ThrowAsync<NullEntryException>();
    }

    [Fact]
    public async Task Create_should_throw_when_requestAmount_is_zero()
    {
        Func<Task> bpd = () => _builder.WithRequestAmount(new Money(0)).Build();

        await bpd.Should().ThrowAsync<InvalidRequestAmountException>();
    }


    [Fact]
    public async Task Create_should_throw_when_eventPublisher_is_null()
    {
        Func<Task> bpd = () => _builder.WithEventPublisher(null).Build();

        await bpd.Should().ThrowAsync<NullEntryException>();
    }

    [Fact]
    public async Task PspPaymentStatus_values_are_persisted_contract_and_should_not_change()
    {
        ((int)PspPaymentStatus.Initiated).Should().Be(1);
        ((int)PspPaymentStatus.TokenReceived).Should().Be(2);
        ((int)PspPaymentStatus.CallbackReceived).Should().Be(3);
        ((int)PspPaymentStatus.Verified).Should().Be(4);
        ((int)PspPaymentStatus.Failed).Should().Be(5);
        ((int)PspPaymentStatus.Expired).Should().Be(6);
        ((int)PspPaymentStatus.Reversed).Should().Be(7);

        Enum.GetValues<PspPaymentStatus>().Should().HaveCount(7,
            "adding a member requires updating the Status check constraint and any DDL");
    }
}
