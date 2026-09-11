using FinanceSystem.Domain.BankPaymentDetails.Enums;
using FinanceSystem.Domain.BankPaymentDetails.Exceptions;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using FluentAssertions;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.Tests.Unit.BankPaymentDetailUnitTests;

public class BankPaymentDetailTests
{
    private readonly BankPaymentDetailBuilder _builder = new();

    [Fact]
    public async Task Create_should_properly_create_bankPaymentDetail()
    {
        var bpd = await _builder.Build();

        bpd.PspId.Should().Be(Guid.Parse(BankPaymentDetailBuilder.DefaultGuidString));
        bpd.Status.Should().Be(BankPaymentStatus.Initiated);
        bpd.RequestAmountRial.Should().Be(BankPaymentDetailBuilder.DefaultRequestAmountRial);
        bpd.RedirectedAmountRial.Should().BeNull();
        bpd.TargetAccountId.Should().Be(BankPaymentDetailBuilder.DefaultTargetAccountId);
        bpd.Authority.Should().Be(BankPaymentDetailBuilder.DefaultAuthority);
        bpd.RRN.Should().BeNull();
        bpd.RefNum.Should().BeNull();
        bpd.TraceNumber.Should().BeNull();
        bpd.MaskedPan.Should().BeNull();
        bpd.ResultCode.Should().BeNull();
        bpd.RawCallback.Should().BeNull();
        bpd.VerifiedAtUtc.Should().BeNull();
    }

    [Fact]
    public async Task Create_should_throw_when_pspId_is_empty_guid()
    {
        Func<Task> bpd = () => _builder.WithPspId(Guid.Empty).Build();

        await bpd.Should().ThrowAsync<InvalidPspIdException>();
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
        Func<Task> bpd = () => _builder.WithRequestAmount(new Money(0, Currency.Rial)).Build();

        await bpd.Should().ThrowAsync<InvalidRequestAmountException>();
    }

    [Fact]
    public async Task Create_should_throw_when_requestAmount_currency_is_not_rial()
    {
        Func<Task> bpd = () => _builder
            .WithRequestAmount(new Money(BankPaymentDetailBuilder.DefaultRequestAmountRial, Currency.Toman))
            .Build();

        await bpd.Should().ThrowAsync<InvalidMoneyCurrencyException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public async Task Create_should_throw_when_authority_is_null_or_empty_or_whiteSpace(string? authority)
    {
        Func<Task> bpd = () => _builder.WithAuthority(authority).Build();

        await bpd.Should().ThrowAsync<InvalidAuthorityException>();
    }

    [Fact]
    public async Task BankPaymentStatus_values_are_persisted_contract_and_should_not_change()
    {
        ((int)BankPaymentStatus.Initiated).Should().Be(1);
        ((int)BankPaymentStatus.CallbackReceived).Should().Be(2);
        ((int)BankPaymentStatus.Verified).Should().Be(3);
        ((int)BankPaymentStatus.Failed).Should().Be(4);
        ((int)BankPaymentStatus.Expired).Should().Be(5);
        ((int)BankPaymentStatus.Reversed).Should().Be(6);
    }
}
