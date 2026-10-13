using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Application.Contracts.Payments.Events;
using FinanceSystem.Application.Payments.CommandHandlers;
using FinanceSystem.Application.Payments.Gateways;
using FinanceSystem.Domain.AccountEntries;
using FinanceSystem.Domain.AccountEntries.Enums;
using FinanceSystem.Domain.FinanceAccounts;
using FinanceSystem.Domain.FinanceAccounts.Enums;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using FinanceSystem.Domain.Payments.Exceptions;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.Tests.Unit.PaymentUnitTests;
using FluentAssertions;
using NSubstitute;
using Shared.Core;
using Shared.Core.Events;
using Shared.Domain;

namespace FinanceSystem.Application.Tests.Unit.PaymentUnitTest;

public class TopUpCallbackCommandHandlerTests
{
    private const long PaymentId = 42;
    private const long UserAccountId = 22;
    private const long CompanyWalletId = 11;
    private const string Token = "token-123";
    private const string ReferenceNumber = "ref-1";

    private readonly IPaymentRepository _paymentRepository = Substitute.For<IPaymentRepository>();
    private readonly IFinanceAccountRepository _financeAccountRepository = Substitute.For<IFinanceAccountRepository>();
    private readonly IAccountEntryRepository _accountEntryRepository = Substitute.For<IAccountEntryRepository>();
    private readonly IPspGatewayFactory _pspGatewayFactory = Substitute.For<IPspGatewayFactory>();
    private readonly IPspGateway _pspGateway = Substitute.For<IPspGateway>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IEventPublisher _publisher = Substitute.For<IEventPublisher>();
    private readonly PspCallback _callback = MakeSuccessfulCallback();
    private readonly TopUpCallbackCommandHandler _handler;

    public TopUpCallbackCommandHandlerTests()
    {
        _pspGateway.Code.Returns(PspCode.Saman);
        _pspGateway.ParseCallback(Arg.Any<IReadOnlyDictionary<string, string>>()).Returns(_ => _callback);
        _pspGatewayFactory.Get(PspCode.Saman).Returns(_pspGateway);

        _handler = new TopUpCallbackCommandHandler(
            _paymentRepository,
            _financeAccountRepository,
            _accountEntryRepository,
            _pspGatewayFactory,
            _unitOfWork,
            _publisher,
            Substitute.For<IEventListener>());
    }

    [Fact]
    public async Task Handle_should_throw_when_payment_is_not_found()
    {
        Func<Task> act = () => _handler.Handle(MakeCommand());

        await act.Should().ThrowAsync<PaymentNotFoundException>();
        await _unitOfWork.DidNotReceive().SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_should_throw_and_leave_payment_unchanged_when_psp_code_does_not_match()
    {
        var payment = await ArrangePaymentWithToken();
        var otherGateway = Substitute.For<IPspGateway>();
        otherGateway.Code.Returns(PspCode.BehPardakht);
        otherGateway.ParseCallback(Arg.Any<IReadOnlyDictionary<string, string>>()).Returns(_callback);
        _pspGatewayFactory.Get(PspCode.BehPardakht).Returns(otherGateway);
        var command = MakeCommand();
        command.PspCode = PspCode.BehPardakht;

        Func<Task> act = () => _handler.Handle(command);

        await act.Should().ThrowAsync<PspCodeMismatchException>();
        payment.State.Should().Be(PaymentState.TokenReceived);
    }

    [Fact]
    public async Task Handle_should_throw_and_leave_payment_unchanged_when_token_does_not_match()
    {
        var payment = await ArrangePaymentWithToken();
        _callback.Token = "forged-token";

        Func<Task> act = () => _handler.Handle(MakeCommand());

        await act.Should().ThrowAsync<CallbackTokenMismatchException>();
        payment.State.Should().Be(PaymentState.TokenReceived);
    }

    [Fact]
    public async Task Handle_should_report_success_without_verifying_again_when_payment_already_succeeded()
    {
        var payment = await ArrangePaymentWithToken();
        payment.StoreCallback("OK", 0, ReferenceNumber, PaymentBuilder.DefaultAmount, null, null, null);
        payment.MarkVerifying();
        await payment.MarkSucceeded();

        await _handler.Handle(MakeCommand());

        payment.State.Should().Be(PaymentState.Succeeded);
        await _pspGateway.DidNotReceiveWithAnyArgs().VerifyPaymentAsync(default!, default);
        await AssertOutcome(TopUpOutcome.Success);
    }

    [Fact]
    public async Task Handle_should_throw_and_leave_payment_unchanged_when_state_is_not_token_received()
    {
        var payment = await ArrangePaymentWithToken();
        payment.StoreCallback("OK", 2, ReferenceNumber, PaymentBuilder.DefaultAmount - 1, "", "", "");
        await payment.MarkFailed(PaymentFailureReason.CallbackAmountMismatch);

        Func<Task> act = () => _handler.Handle(MakeCommand());

        await act.Should().ThrowAsync<InvalidPaymentStateException>();
        payment.State.Should().Be(PaymentState.Failed);
        payment.FailureReason.Should().Be(PaymentFailureReason.CallbackAmountMismatch);
    }

    [Fact]
    public async Task Handle_should_store_callback_and_report_failure_without_verifying_when_callback_is_not_successful()
    {
        var payment = await ArrangePaymentWithToken();
        _callback.IsSuccess = false;
        _callback.Status = "SessionIsNull";
        _callback.StatusCode = 4;
        _callback.ReferenceNumber = string.Empty;

        await _handler.Handle(MakeCommand());

        payment.Status.Should().Be("SessionIsNull");
        payment.StatusCode.Should().Be(4);
        await _pspGateway.DidNotReceiveWithAnyArgs().VerifyPaymentAsync(default!, default);
        await AssertOutcome(TopUpOutcome.Failure);
    }

    [Fact]
    public async Task Handle_should_mark_payment_failed_when_callback_amount_does_not_match()
    {
        var payment = await ArrangePaymentWithToken();
        _callback.Amount = PaymentBuilder.DefaultAmount - 1;

        await _handler.Handle(MakeCommand());

        payment.State.Should().Be(PaymentState.Failed);
        payment.FailureReason.Should().Be(PaymentFailureReason.CallbackAmountMismatch);
        await _pspGateway.DidNotReceiveWithAnyArgs().VerifyPaymentAsync(default!, default);
        await AssertOutcome(TopUpOutcome.Failure);
    }

    [Fact]
    public async Task Handle_should_throw_and_leave_payment_unchanged_when_reference_number_is_already_used()
    {
        var payment = await ArrangePaymentWithToken();
        var otherPayment = await new PaymentBuilder().Build();
        _paymentRepository.GetByReferenceNumberAsync(ReferenceNumber, Arg.Any<CancellationToken>()).Returns(otherPayment);

        Func<Task> act = () => _handler.Handle(MakeCommand());

        await act.Should().ThrowAsync<DuplicateReferenceNumberException>();
        payment.State.Should().Be(PaymentState.TokenReceived);
        payment.ReferenceNumber.Should().BeNull();
    }

    [Fact]
    public async Task Handle_should_save_verifying_state_before_sending_verify_request()
    {
        var payment = await ArrangePaymentWithToken();
        PaymentState? stateWhenSaved = null;
        _unitOfWork.When(u => u.SaveChangesAsync()).Do(_ => stateWhenSaved = payment.State);
        ArrangeVerification(new PaymentVerificationResponse { IsSuccess = false });

        await _handler.Handle(MakeCommand());

        stateWhenSaved.Should().Be(PaymentState.Verifying);
        Received.InOrder(() =>
        {
            _unitOfWork.SaveChangesAsync();
            _pspGateway.VerifyPaymentAsync(Arg.Any<PaymentVerificationRequest>(), Arg.Any<CancellationToken>());
        });
    }

    [Fact]
    public async Task Handle_should_mark_payment_failed_when_psp_rejects_verification()
    {
        var payment = await ArrangePaymentWithToken();
        ArrangeVerification(new PaymentVerificationResponse { IsSuccess = false });

        await _handler.Handle(MakeCommand());

        payment.State.Should().Be(PaymentState.Failed);
        payment.FailureReason.Should().Be(PaymentFailureReason.VerificationFailed);
        await AssertOutcome(TopUpOutcome.Failure);
    }

    [Fact]
    public async Task Handle_should_mark_payment_succeeded_and_credit_user_wallet_when_verification_succeeds()
    {
        var payment = await ArrangePaymentWithToken();
        var (userAccount, companyWallet) = ArrangeFinanceAccounts();
        ArrangeVerification(VerifiedResponse(PaymentBuilder.DefaultAmount));

        await _handler.Handle(MakeCommand());

        payment.State.Should().Be(PaymentState.Succeeded);
        payment.SucceededAtUtc.Should().NotBeNull();
        payment.ReferenceNumber.Should().Be(ReferenceNumber);
        userAccount.CachedBalance.Should().Be(PaymentBuilder.DefaultAmount);
        companyWallet.CachedBalance.Should().Be(-PaymentBuilder.DefaultAmount);
        await _accountEntryRepository.Received(1).AddAsync(Arg.Is<AccountEntry>(e =>
            e.PaymentId == PaymentId &&
            e.FinanceAccountId == UserAccountId &&
            e.Amount == PaymentBuilder.DefaultAmount &&
            e.Direction == EntryDirection.In));
        await _accountEntryRepository.Received(1).AddAsync(Arg.Is<AccountEntry>(e =>
            e.PaymentId == PaymentId &&
            e.FinanceAccountId == CompanyWalletId &&
            e.Amount == PaymentBuilder.DefaultAmount &&
            e.Direction == EntryDirection.Out));
        await AssertOutcome(TopUpOutcome.Success);
    }

    private async Task<Payment> ArrangePayment()
    {
        var payment = await new PaymentBuilder()
            .WithPurpose(PaymentPurpose.TopUp)
            .WithChannel(PaymentChannel.Psp)
            .WithPspCode(PspCode.Saman)
            .Build();
        SetId(payment, PaymentId);

        _paymentRepository.GetByIdAsync(PaymentId, Arg.Any<CancellationToken>()).Returns(payment);

        return payment;
    }

    private async Task<Payment> ArrangePaymentWithToken()
    {
        var payment = await ArrangePayment();
        await payment.MarkTokenReceived(Token, "https://ipg.example/pay");
        return payment;
    }

    private (FinanceAccount UserAccount, FinanceAccount CompanyWallet) ArrangeFinanceAccounts()
    {
        var userAccount = new FinanceAccount(
            FinanceAccountType.UserWallet, FinanceAccountStatus.Active, Guid.NewGuid(), new Money(0), DateTimeOffset.UtcNow, false);
        var companyWallet = new FinanceAccount(
            FinanceAccountType.CompanyWallet, FinanceAccountStatus.Active, SystemFinanceAccounts.CompanyUserId, new Money(0), DateTimeOffset.UtcNow, true);
        SetId(userAccount, UserAccountId);
        SetId(companyWallet, CompanyWalletId);

        _financeAccountRepository.GetByIdAsync(UserAccountId, Arg.Any<CancellationToken>()).Returns(userAccount);
        _financeAccountRepository.GetByIdAsync(CompanyWalletId, Arg.Any<CancellationToken>()).Returns(companyWallet);

        return (userAccount, companyWallet);
    }

    private void ArrangeVerification(PaymentVerificationResponse response)
    {
        _pspGateway.VerifyPaymentAsync(Arg.Any<PaymentVerificationRequest>(), Arg.Any<CancellationToken>()).Returns(response);
    }

    private static PaymentVerificationResponse VerifiedResponse(decimal amount)
    {
        return new PaymentVerificationResponse { IsSuccess = true, Amount = amount };
    }

    private static TopUpCallbackCommand MakeCommand()
    {
        return new TopUpCallbackCommand
        {
            PspCode = PspCode.Saman
        };
    }

    private static PspCallback MakeSuccessfulCallback()
    {
        return new PspCallback
        {
            ReservationNumber = PaymentId,
            IsSuccess = true,
            Status = "OK",
            StatusCode = 2,
            Token = Token,
            ReferenceNumber = ReferenceNumber,
            Amount = PaymentBuilder.DefaultAmount
        };
    }

    private async Task AssertOutcome(TopUpOutcome outcome)
    {
        await _publisher.Received(1).Publish(Arg.Is<TopUpCallbackHandledEvent>(e => e.Outcome == outcome));
    }

    private static void SetId<TKey>(EntityBase<TKey> entity, TKey id)
    {
        typeof(EntityBase<TKey>).GetProperty(nameof(EntityBase<TKey>.Id))!.SetValue(entity, id);
    }
}
