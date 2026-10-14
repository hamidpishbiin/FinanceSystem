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
        payment.FinanceAccountId.Should().Be(PaymentBuilder.DefaultFinanceAccountId);
        payment.Direction.Should().Be(PaymentBuilder.DefaultDirection);
        payment.ServiceId.Should().Be(PaymentBuilder.DefaultServiceId);
        payment.ReferenceId.Should().Be(PaymentBuilder.DefaultReferenceId);
        payment.State.Should().Be(PaymentState.Initiated);
        payment.PspCode.Should().BeNull();
        payment.Publisher.Should().BeSameAs(_builder.EventPublisher);
    }

    [Fact]
    public async Task Create_should_publish_paymentCreatedEvent_carrying_referenceId()
    {
        await _builder.Build();

        await _builder.EventPublisher
            .Received(1)
            .Publish(Arg.Is<PaymentCreatedEvent>(e =>
                e.ReferenceId == PaymentBuilder.DefaultReferenceId));
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
    public async Task Create_should_throw_when_financeAccountId_is_not_positive(long financeAccountId)
    {
        Func<Task> act = () => _builder.WithFinanceAccountId(financeAccountId).Build();

        await act.Should().ThrowAsync<InvalidFinanceAccountIdException>();
    }

    [Theory]
    [InlineData((PaymentDirection)0)]
    [InlineData((PaymentDirection)99)]
    public async Task Create_should_throw_when_direction_is_not_defined(PaymentDirection direction)
    {
        Func<Task> act = () => _builder.WithDirection(direction).Build();

        await act.Should().ThrowAsync<InvalidPaymentDirectionException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Create_should_throw_when_serviceId_is_null_or_empty_or_whiteSpace(string? serviceId)
    {
        Func<Task> act = () => _builder.WithServiceId(serviceId).Build();

        await act.Should().ThrowAsync<InvalidServiceIdException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Create_should_throw_when_referenceId_is_null_or_empty_or_whiteSpace(string? referenceId)
    {
        Func<Task> act = () => _builder.WithReferenceId(referenceId).Build();

        await act.Should().ThrowAsync<InvalidReferenceIdException>();
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
        payment.State.Should().Be(PaymentState.Initiated);
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
    public void PaymentState_values_are_persisted_contract_and_should_not_change()
    {
        ((byte)PaymentState.Initiated).Should().Be(1);
        ((byte)PaymentState.TokenReceived).Should().Be(2);
        ((byte)PaymentState.CallbackReceived).Should().Be(3);
        ((byte)PaymentState.Succeeded).Should().Be(4);
        ((byte)PaymentState.Failed).Should().Be(5);
        ((byte)PaymentState.Expired).Should().Be(6);
        ((byte)PaymentState.Reversed).Should().Be(7);
        ((byte)PaymentState.Verifying).Should().Be(8);

        Enum.GetValues<PaymentState>().Should().HaveCount(8);
    }

    [Fact]
    public async Task MarkTokenReceived_should_store_token_set_state_and_publish_event()
    {
        var payment = await BuildPspPayment();

        await payment.MarkTokenReceived("token-123", "https://ipg.example/pay");

        payment.Token.Should().Be("token-123");
        payment.State.Should().Be(PaymentState.TokenReceived);
        await _builder.EventPublisher
            .Received(1)
            .Publish(Arg.Is<PspTokenReceivedEvent>(e =>
                e.ReferenceId == PaymentBuilder.DefaultReferenceId &&
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
    public async Task MarkTokenRequestFailed_should_store_failure_details_set_state_and_publish_event()
    {
        var payment = await BuildPspPayment();

        await payment.MarkTokenRequestFailed(PaymentFailureReason.Unknown, "-3", 3, "invalid amount");

        payment.State.Should().Be(PaymentState.Failed);
        payment.FailureReason.Should().Be(PaymentFailureReason.Unknown);
        payment.Status.Should().Be("-3");
        payment.StatusCode.Should().Be(3);
        payment.RawDescription.Should().Be("invalid amount");
        await _builder.EventPublisher
            .Received(1)
            .Publish(Arg.Is<PaymentTokenRequestFailedEvent>(e =>
                e.ReferenceId == PaymentBuilder.DefaultReferenceId));
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

        Func<Task> act = () => payment.MarkTokenRequestFailed(PaymentFailureReason.Unknown, null, null, null);

        await act.Should().ThrowAsync<InvalidPaymentStateException>();
    }

    [Fact]
    public async Task MarkTokenReceived_should_throw_when_state_is_not_initiated()
    {
        var payment = await BuildPspPayment();
        await payment.MarkTokenRequestFailed(PaymentFailureReason.Unknown, null, null, null);

        Func<Task> act = () => payment.MarkTokenReceived("token-123", "https://ipg.example/pay");

        await act.Should().ThrowAsync<InvalidPaymentStateException>();
    }

    [Fact]
    public async Task MarkTokenRequestFailed_should_throw_when_state_is_not_initiated()
    {
        var payment = await BuildPspPayment();
        await payment.MarkTokenReceived("token-123", "https://ipg.example/pay");

        Func<Task> act = () => payment.MarkTokenRequestFailed(PaymentFailureReason.Unknown, null, null, null);

        await act.Should().ThrowAsync<InvalidPaymentStateException>();
    }

    [Fact]
    public async Task StoreCallback_should_store_callback_data_and_set_state_to_callbackReceived()
    {
        var payment = await BuildPaymentWithToken();

        payment.StoreCallback("OK", 2, "ref-1", PaymentBuilder.DefaultAmount, "6037-****-1234", "rrn-1", "trace-1");

        payment.State.Should().Be(PaymentState.CallbackReceived);
        payment.Status.Should().Be("OK");
        payment.StatusCode.Should().Be(2);
        payment.ReferenceNumber.Should().Be("ref-1");
        payment.RedirectedAmount.Should().Be(PaymentBuilder.DefaultAmount);
        payment.MaskedPan.Should().Be("6037-****-1234");
        payment.RRN.Should().Be("rrn-1");
        payment.TraceNumber.Should().Be("trace-1");
    }

    [Fact]
    public async Task StoreCallback_should_throw_when_state_is_not_tokenReceived()
    {
        var payment = await BuildPspPayment();

        Action act = () => payment.StoreCallback("OK", 2, "ref-1", PaymentBuilder.DefaultAmount, null, null, null);

        act.Should().Throw<InvalidPaymentStateException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task StoreCallback_should_throw_when_rawStatus_is_null_or_empty(string? rawStatus)
    {
        var payment = await BuildPaymentWithToken();

        Action act = () => payment.StoreCallback(rawStatus!, 2, "ref-1", PaymentBuilder.DefaultAmount, null, null, null);

        act.Should().Throw<InvalidRawStatusException>();
    }

    [Fact]
    public async Task MarkFailed_should_set_state_and_failure_reason_and_publish_event()
    {
        var payment = await BuildPaymentWithCallback();

        await payment.MarkFailed(PaymentFailureReason.CallbackAmountMismatch);

        payment.State.Should().Be(PaymentState.Failed);
        payment.FailureReason.Should().Be(PaymentFailureReason.CallbackAmountMismatch);
        await _builder.EventPublisher
            .Received(1)
            .Publish(Arg.Is<PaymentFailedEvent>(e =>
                e.ReferenceId == PaymentBuilder.DefaultReferenceId));
    }

    [Fact]
    public async Task MarkFailed_should_be_allowed_while_verifying()
    {
        var payment = await BuildPaymentWithCallback();
        payment.MarkVerifying();

        await payment.MarkFailed(PaymentFailureReason.VerificationFailed);

        payment.State.Should().Be(PaymentState.Failed);
        payment.FailureReason.Should().Be(PaymentFailureReason.VerificationFailed);
    }

    [Fact]
    public async Task MarkFailed_should_throw_when_callback_is_not_received()
    {
        var payment = await BuildPaymentWithToken();

        Func<Task> act = () => payment.MarkFailed(PaymentFailureReason.CallbackAmountMismatch);

        await act.Should().ThrowAsync<InvalidPaymentStateException>();
    }

    [Fact]
    public async Task MarkVerifying_should_set_state_to_verifying()
    {
        var payment = await BuildPaymentWithCallback();

        payment.MarkVerifying();

        payment.State.Should().Be(PaymentState.Verifying);
    }

    [Fact]
    public async Task MarkVerifying_should_throw_when_callback_is_not_received()
    {
        var payment = await BuildPaymentWithToken();

        Action act = () => payment.MarkVerifying();

        act.Should().Throw<InvalidPaymentStateException>();
    }

    [Fact]
    public async Task MarkSucceeded_should_set_state_and_success_time_and_publish_event()
    {
        var payment = await BuildPaymentWithCallback();
        payment.MarkVerifying();

        await payment.MarkSucceeded();

        payment.State.Should().Be(PaymentState.Succeeded);
        payment.SucceededAtUtc.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
        await _builder.EventPublisher
            .Received(1)
            .Publish(Arg.Is<PaymentSucceededEvent>(e =>
                e.ReferenceId == PaymentBuilder.DefaultReferenceId));
    }

    [Fact]
    public async Task MarkSucceeded_should_throw_when_state_is_not_verifying()
    {
        var payment = await BuildPaymentWithCallback();

        Func<Task> act = () => payment.MarkSucceeded();

        await act.Should().ThrowAsync<InvalidPaymentStateException>();
    }

    [Fact]
    public async Task MarkReversed_should_set_state_and_publish_event()
    {
        var payment = await BuildPaymentWithCallback();
        payment.MarkVerifying();

        await payment.MarkReversed();

        payment.State.Should().Be(PaymentState.Reversed);
        await _builder.EventPublisher
            .Received(1)
            .Publish(Arg.Is<PaymentReversedEvent>(e =>
                e.ReferenceId == PaymentBuilder.DefaultReferenceId));
    }

    [Fact]
    public async Task MarkReversed_should_throw_when_state_is_not_verifying()
    {
        var payment = await BuildPaymentWithCallback();

        Func<Task> act = () => payment.MarkReversed();

        await act.Should().ThrowAsync<InvalidPaymentStateException>();
    }

    private async Task<Payment> BuildPaymentWithToken()
    {
        var payment = await BuildPspPayment();
        await payment.MarkTokenReceived("token-123", "https://ipg.example/pay");
        return payment;
    }

    private async Task<Payment> BuildPaymentWithCallback()
    {
        var payment = await BuildPaymentWithToken();
        payment.StoreCallback("OK", 2, "ref-1", PaymentBuilder.DefaultAmount, null, null, null);
        return payment;
    }

    private Task<Payment> BuildPspPayment()
    {
        return _builder
            .WithChannel(PaymentChannel.Psp)
            .WithPspCode(PaymentBuilder.DefaultPspCode)
            .Build();
    }
}
