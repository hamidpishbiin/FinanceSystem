using FinanceSystem.Domain.Contract.Payments;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using FinanceSystem.Domain.Payments.Exceptions;
using FluentAssertions;
using NSubstitute;
using Shared.Core.Events;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.Tests.Unit.PaymentUnitTests;

public class PaymentTests
{
    private readonly PaymentBuilder _builder = new();

    [Fact]
    public async Task Create_should_properly_create_payment()
    {
        var payment = await _builder.Build();

        payment.IdempotencyKey.Should().Be(PaymentBuilder.DefaultIdempotencyKey);
        payment.Purpose.Should().Be(PaymentBuilder.DefaultPurpose);
        payment.Channel.Should().Be(PaymentBuilder.DefaultChannel);
        payment.AmountRial.Should().Be(PaymentBuilder.DefaultAmountRial);
        payment.SourceAccountId.Should().Be(PaymentBuilder.DefaultSourceAccountId);
        payment.DestinationAccountId.Should().Be(PaymentBuilder.DefaultDestinationAccountId);
        payment.OriginServiceId.Should().Be(PaymentBuilder.DefaultOriginServiceId);
        payment.ExternalReferenceId.Should().Be(PaymentBuilder.DefaultExternalReferenceId);
        payment.ExternalTag.Should().Be(PaymentBuilder.DefaultExternalTag);
        payment.BankPaymentDetailId.Should().BeNull();
        payment.Publisher.Should().BeSameAs(_builder.EventPublisher);
    }

    [Fact]
    public async Task Create_should_publish_paymentCreatedEvent_carrying_idempotencyKey_and_amount()
    {
        await _builder.Build();

        await _builder.EventPublisher
            .Received(1)
            .Publish(Arg.Is<PaymentCreatedEvent>(e =>
                e.IdempotencyKey == PaymentBuilder.DefaultIdempotencyKey &&
                e.AmountRial == PaymentBuilder.DefaultAmountRial));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Create_should_throw_when_idempotencyKey_is_null_or_empty_or_whiteSpace(string? idempotencyKey)
    {
        Func<Task> act = () => _builder.WithIdempotencyKey(idempotencyKey).Build();

        await act.Should().ThrowAsync<InvalidIdempotencyKeyException>();
    }

    [Theory]
    [InlineData((PaymentPurpose)0)]
    [InlineData((PaymentPurpose)99)]
    public async Task Create_should_throw_when_purpose_is_not_defined(PaymentPurpose purpose)
    {
        Func<Task> act = () => _builder.WithPurpose(purpose).Build();

        await act.Should().ThrowAsync<InvalidPaymentPurposeException>();
    }

    [Theory]
    [InlineData((PaymentChannel)0)]
    [InlineData((PaymentChannel)99)]
    public async Task Create_should_throw_when_channel_is_not_defined(PaymentChannel channel)
    {
        Func<Task> act = () => _builder.WithChannel(channel).Build();

        await act.Should().ThrowAsync<InvalidPaymentChannelException>();
    }

    [Fact]
    public async Task Create_should_throw_when_amount_is_null()
    {
        Func<Task> act = () => _builder.WithAmount(null!).Build();

        await act.Should().ThrowAsync<NullEntryException>();
    }

    [Fact]
    public async Task Create_should_throw_when_amount_is_zero()
    {
        Func<Task> act = () => _builder.WithAmount(new Money(0, Currency.Rial)).Build();

        await act.Should().ThrowAsync<InvalidPaymentAmountException>();
    }

    [Fact]
    public async Task Create_should_throw_when_amount_currency_is_not_rial()
    {
        Func<Task> act = () => _builder
            .WithAmount(new Money(PaymentBuilder.DefaultAmountRial, Currency.Toman))
            .Build();

        await act.Should().ThrowAsync<InvalidMoneyCurrencyException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Create_should_throw_when_sourceAccountId_is_not_positive(long sourceAccountId)
    {
        Func<Task> act = () => _builder.WithSourceAccountId(sourceAccountId).Build();

        await act.Should().ThrowAsync<InvalidSourceAccountIdException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Create_should_throw_when_destinationAccountId_is_not_positive(long destinationAccountId)
    {
        Func<Task> act = () => _builder.WithDestinationAccountId(destinationAccountId).Build();

        await act.Should().ThrowAsync<InvalidDestinationAccountIdException>();
    }

    [Fact]
    public async Task Create_should_throw_when_source_and_destination_accounts_are_the_same()
    {
        Func<Task> act = () => _builder
            .WithSourceAccountId(PaymentBuilder.DefaultSourceAccountId)
            .WithDestinationAccountId(PaymentBuilder.DefaultSourceAccountId)
            .Build();

        await act.Should().ThrowAsync<SameSourceAndDestinationAccountException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Create_should_throw_when_originServiceId_is_null_or_empty_or_whiteSpace(string? originServiceId)
    {
        Func<Task> act = () => _builder.WithOriginServiceId(originServiceId).Build();

        await act.Should().ThrowAsync<InvalidOriginServiceIdException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Create_should_throw_when_externalReferenceId_is_null_or_empty_or_whiteSpace(string? externalReferenceId)
    {
        Func<Task> act = () => _builder.WithExternalReferenceId(externalReferenceId).Build();

        await act.Should().ThrowAsync<InvalidExternalReferenceIdException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Create_should_throw_when_externalTag_is_null_or_empty_or_whiteSpace(string? externalTag)
    {
        Func<Task> act = () => _builder.WithExternalTag(externalTag).Build();

        await act.Should().ThrowAsync<InvalidExternalTagException>();
    }

    [Fact]
    public async Task Create_should_throw_when_channel_is_bank_and_bankPaymentDetailId_is_missing()
    {
        Func<Task> act = () => _builder
            .WithChannel(PaymentChannel.Bank)
            .WithBankPaymentDetailId(null)
            .Build();

        await act.Should().ThrowAsync<MissingBankPaymentDetailException>();
    }

    [Fact]
    public async Task Create_should_throw_when_channel_is_not_bank_and_bankPaymentDetailId_is_supplied()
    {
        Func<Task> act = () => _builder
            .WithChannel(PaymentChannel.Wallet)
            .WithBankPaymentDetailId(PaymentBuilder.DefaultBankPaymentDetailId)
            .Build();

        await act.Should().ThrowAsync<UnexpectedBankPaymentDetailException>();
    }

    [Fact]
    public async Task Create_should_succeed_when_channel_is_bank_and_bankPaymentDetailId_is_supplied()
    {
        var payment = await _builder
            .WithChannel(PaymentChannel.Bank)
            .WithBankPaymentDetailId(PaymentBuilder.DefaultBankPaymentDetailId)
            .Build();

        payment.Channel.Should().Be(PaymentChannel.Bank);
        payment.BankPaymentDetailId.Should().Be(PaymentBuilder.DefaultBankPaymentDetailId);
    }

    [Fact]
    public async Task Create_should_throw_when_eventPublisher_is_null()
    {
        Func<Task> act = () => _builder.WithEventPublisher(null!).Build();

        await act.Should().ThrowAsync<NullEntryException>();
    }

    [Fact]
    public void PaymentPurpose_values_are_persisted_contract_and_should_not_change()
    {
        ((byte)PaymentPurpose.TopUp).Should().Be(1);
        ((byte)PaymentPurpose.Purchase).Should().Be(2);
        ((byte)PaymentPurpose.Withdrawal).Should().Be(3);
        ((byte)PaymentPurpose.Refund).Should().Be(4);
        ((byte)PaymentPurpose.Transfer).Should().Be(5);
        ((byte)PaymentPurpose.Adjustment).Should().Be(6);

        Enum.GetValues<PaymentPurpose>().Should().HaveCount(6,
            "adding a member requires updating the Purpose check constraint and any DDL");
    }

    [Fact]
    public void PaymentChannel_values_are_persisted_contract_and_should_not_change()
    {
        ((byte)PaymentChannel.Wallet).Should().Be(1);
        ((byte)PaymentChannel.Bank).Should().Be(2);

        Enum.GetValues<PaymentChannel>().Should().HaveCount(2,
            "adding a member requires revisiting the Bank/BankPaymentDetailId pairing rules in Payment.Create");
    }
}
