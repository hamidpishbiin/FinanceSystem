using FinanceSystem.Domain.Contract.Payments;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using FinanceSystem.Domain.Payments.Exceptions;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Exceptions;
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

        payment.Purpose.Should().Be(PaymentBuilder.DefaultPurpose);
        payment.Channel.Should().Be(PaymentBuilder.DefaultChannel);
        payment.Amount.Should().Be(PaymentBuilder.DefaultAmount);
        payment.SourceFinanceAccountId.Should().Be(PaymentBuilder.DefaultSourceFinanceAccountId);
        payment.DestinationFinanceAccountId.Should().Be(PaymentBuilder.DefaultDestinationFinanceAccountId);
        payment.OriginServiceId.Should().Be(PaymentBuilder.DefaultOriginServiceId);
        payment.ExternalReferenceId.Should().Be(PaymentBuilder.DefaultExternalReferenceId);
        payment.ExternalTag.Should().Be(PaymentBuilder.DefaultExternalTag);
        payment.Status.Should().Be(PaymentStatus.Initiated);
        payment.PspCode.Should().BeNull();
        payment.Publisher.Should().BeSameAs(_builder.EventPublisher);
    }

    [Fact]
    public async Task Create_should_publish_paymentCreatedEvent_carrying_externalReferenceId_and_amount()
    {
        await _builder.Build();

        await _builder.EventPublisher
            .Received(1)
            .Publish(Arg.Is<PaymentCreatedEvent>(e =>
                e.ExternalReferenceId == PaymentBuilder.DefaultExternalReferenceId &&
                e.Amount == PaymentBuilder.DefaultAmount));
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
        Func<Task> act = () => _builder.WithAmount(new Money(0)).Build();

        await act.Should().ThrowAsync<InvalidPaymentAmountException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Create_should_throw_when_sourceFinanceAccountId_is_not_positive(long sourceFinanceAccountId)
    {
        Func<Task> act = () => _builder.WithSourceFinanceAccountId(sourceFinanceAccountId).Build();

        await act.Should().ThrowAsync<InvalidSourceFinanceAccountIdException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Create_should_throw_when_destinationFinanceAccountId_is_not_positive(long destinationFinanceAccountId)
    {
        Func<Task> act = () => _builder.WithDestinationFinanceAccountId(destinationFinanceAccountId).Build();

        await act.Should().ThrowAsync<InvalidDestinationFinanceAccountIdException>();
    }

    [Fact]
    public async Task Create_should_throw_when_source_and_destination_finance_accounts_are_the_same()
    {
        Func<Task> act = () => _builder
            .WithSourceFinanceAccountId(PaymentBuilder.DefaultSourceFinanceAccountId)
            .WithDestinationFinanceAccountId(PaymentBuilder.DefaultSourceFinanceAccountId)
            .Build();

        await act.Should().ThrowAsync<SameSourceAndDestinationFinanceAccountException>();
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
    public async Task Create_should_throw_when_channel_is_psp_and_pspCode_is_missing()
    {
        Func<Task> act = () => _builder
            .WithChannel(PaymentChannel.Psp)
            .WithPspCode(null)
            .Build();

        await act.Should().ThrowAsync<MissingPspCodeException>();
    }

    [Fact]
    public async Task Create_should_throw_when_channel_is_not_psp_and_pspCode_is_supplied()
    {
        Func<Task> act = () => _builder
            .WithChannel(PaymentChannel.Wallet)
            .WithPspCode(PaymentBuilder.DefaultPspCode)
            .Build();

        await act.Should().ThrowAsync<UnexpectedPspCodeException>();
    }

    [Fact]
    public async Task Create_should_throw_when_pspCode_is_not_defined()
    {
        Func<Task> act = () => _builder
            .WithChannel(PaymentChannel.Psp)
            .WithPspCode((PspCode)99)
            .Build();

        await act.Should().ThrowAsync<InvalidPspCodeException>();
    }

    [Fact]
    public async Task Create_should_succeed_when_channel_is_psp_and_pspCode_is_supplied()
    {
        var payment = await _builder
            .WithChannel(PaymentChannel.Psp)
            .WithPspCode(PaymentBuilder.DefaultPspCode)
            .Build();

        payment.Channel.Should().Be(PaymentChannel.Psp);
        payment.PspCode.Should().Be(PaymentBuilder.DefaultPspCode);
        payment.Status.Should().Be(PaymentStatus.Initiated);
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
        ((byte)PaymentChannel.Psp).Should().Be(2);

        Enum.GetValues<PaymentChannel>().Should().HaveCount(2,
            "adding a member requires revisiting the Psp/PspCode pairing rules in Payment.Create");
    }

    [Fact]
    public void PaymentStatus_values_are_persisted_contract_and_should_not_change()
    {
        ((byte)PaymentStatus.Initiated).Should().Be(1);
        ((byte)PaymentStatus.TokenReceived).Should().Be(2);
        ((byte)PaymentStatus.CallbackReceived).Should().Be(3);
        ((byte)PaymentStatus.Verified).Should().Be(4);
        ((byte)PaymentStatus.Failed).Should().Be(5);
        ((byte)PaymentStatus.Expired).Should().Be(6);
        ((byte)PaymentStatus.Reversed).Should().Be(7);

        Enum.GetValues<PaymentStatus>().Should().HaveCount(7);
    }

    [Fact]
    public async Task MarkTokenReceived_should_store_token_set_status_and_publish_event()
    {
        var payment = await BuildPspPayment();

        await payment.MarkTokenReceived("token-123", "https://ipg.example/pay");

        payment.Token.Should().Be("token-123");
        payment.Status.Should().Be(PaymentStatus.TokenReceived);
        await _builder.EventPublisher
            .Received(1)
            .Publish(Arg.Is<PaymentTokenReceivedEvent>(e =>
                e.ExternalReferenceId == PaymentBuilder.DefaultExternalReferenceId &&
                e.Amount == PaymentBuilder.DefaultAmount &&
                e.IpgUrl == "https://ipg.example/pay"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task MarkTokenReceived_should_throw_when_token_is_null_or_empty_or_whitespace(string? token)
    {
        var payment = await BuildPspPayment();

        Func<Task> act = () => payment.MarkTokenReceived(token!, "https://ipg.example/pay");

        await act.Should().ThrowAsync<InvalidTokenException>();
    }

    [Fact]
    public async Task MarkTokenRequestFailed_should_store_failure_details_set_status_and_publish_event()
    {
        var payment = await BuildPspPayment();

        await payment.MarkTokenRequestFailed(PspFailureReason.InvalidAmount, "-3", "E3", "invalid amount");

        payment.Status.Should().Be(PaymentStatus.Failed);
        payment.FailureReason.Should().Be(PspFailureReason.InvalidAmount);
        payment.RawStatus.Should().Be("-3");
        payment.RawErrorCode.Should().Be("E3");
        payment.RawDescription.Should().Be("invalid amount");
        await _builder.EventPublisher
            .Received(1)
            .Publish(Arg.Is<PaymentTokenRequestFailedEvent>(e =>
                e.ExternalReferenceId == PaymentBuilder.DefaultExternalReferenceId &&
                e.Amount == PaymentBuilder.DefaultAmount));
    }

    [Fact]
    public async Task MarkTokenReceived_should_throw_when_channel_is_not_psp()
    {
        var payment = await _builder.Build();

        Func<Task> act = () => payment.MarkTokenReceived("token-123", "https://ipg.example/pay");

        await act.Should().ThrowAsync<InvalidPaymentStateException>();
    }

    [Fact]
    public async Task MarkTokenRequestFailed_should_throw_when_channel_is_not_psp()
    {
        var payment = await _builder.Build();

        Func<Task> act = () => payment.MarkTokenRequestFailed(PspFailureReason.Unknown, null, null, null);

        await act.Should().ThrowAsync<InvalidPaymentStateException>();
    }

    [Fact]
    public async Task MarkTokenReceived_should_throw_when_status_is_not_initiated()
    {
        var payment = await BuildPspPayment();
        await payment.MarkTokenRequestFailed(PspFailureReason.Unknown, null, null, null);

        Func<Task> act = () => payment.MarkTokenReceived("token-123", "https://ipg.example/pay");

        await act.Should().ThrowAsync<InvalidPaymentStateException>();
    }

    [Fact]
    public async Task MarkTokenRequestFailed_should_throw_when_status_is_not_initiated()
    {
        var payment = await BuildPspPayment();
        await payment.MarkTokenReceived("token-123", "https://ipg.example/pay");

        Func<Task> act = () => payment.MarkTokenRequestFailed(PspFailureReason.Unknown, null, null, null);

        await act.Should().ThrowAsync<InvalidPaymentStateException>();
    }

    private Task<Payment> BuildPspPayment()
    {
        return _builder
            .WithChannel(PaymentChannel.Psp)
            .WithPspCode(PaymentBuilder.DefaultPspCode)
            .Build();
    }
}
