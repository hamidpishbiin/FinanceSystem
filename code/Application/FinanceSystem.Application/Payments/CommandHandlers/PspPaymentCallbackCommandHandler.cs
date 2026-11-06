using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Application.Contracts.Payments.Events;
using FinanceSystem.Application.Gateways;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using FinanceSystem.Domain.Payments.Exceptions;
using FinanceSystem.Domain.PspExchangeLogs;
using Microsoft.Extensions.Options;

namespace FinanceSystem.Application.Payments.CommandHandlers;

public class PspPaymentCallbackCommandHandler(
    IPaymentRepository repository,
    IPspGatewayFactory pspGatewayFactory,
    IUnitOfWork unitOfWork,
    IEventPublisher publisher,
    IEventListener listener,
    IOptions<PspOptions> pspOptions,
    PaymentEligibilityService paymentEligibilityService,
    IPspExchangeLogRepository pspExchangeLogRepository)
    : PaymentCommandHandler<ProcessPspPaymentCallbackCommand>(repository, publisher, listener)
{
    public override async Task Handle(ProcessPspPaymentCallbackCommand command, CancellationToken cancellationToken = default)
    {
        var pspGateway = pspGatewayFactory.Get(command.PspCode);

        var callback = pspGateway.ParseCallback(command.Fields);

        var payment = await paymentEligibilityService.GetPurchasePaymentAsync(callback.ReservationNumber, cancellationToken);

        var callbackLog = PspExchangeLog.CreateCallbackLog(payment.Id, command.Fields.ToString());
        pspExchangeLogRepository.Add(callbackLog);

        payment.Publisher = Publisher;

        var resultPageUrl = pspOptions.Value.For(pspGateway.Code).PaymentResultPageUrl;

        if (payment.State == PaymentState.Succeeded)
        {
            await PublishOutcome(PaymentOutcome.Success, resultPageUrl);
            return;
        }

        await paymentEligibilityService.ValidateReferenceNumberAsync(callback.ReferenceNumber, cancellationToken);

        Guard<PspCodeMismatchException>.IsTrue(payment.PspCode != pspGateway.Code);
        Guard<CallbackTokenMismatchException>.IsTrue(payment.Token != callback.Token);

        Guard<InvalidPaymentStateException>.IsTrue(payment.State != PaymentState.TokenReceived);

        payment.StoreCallback(
            callback.ReferenceNumber,
            callback.Amount,
            callback.MaskedPan,
            callback.RRN,
            callback.TraceNumber);

        if (!callback.IsSuccess)
        {
            await FailAsync(payment, resultPageUrl);
            return;
        }

        if (callback.Amount != payment.Amount)
        {
            await FailAsync(payment, resultPageUrl, PaymentFailureReason.CallbackAmountMismatch);
            return;
        }

        payment.MarkVerifying();

        await unitOfWork.SaveChangesAsync();

        var verificationRequest = new PaymentVerificationRequest()
        {
            ReservationNumber = payment.Id,
            ReferenceNumber = payment.ReferenceNumber!,
            Amount = payment.Amount
        };

        var verification = await pspGateway.VerifyPaymentAsync(verificationRequest, cancellationToken);

        if (!verification.IsSuccess)
        {
            await FailAsync(payment, resultPageUrl, PaymentFailureReason.VerificationFailed);
            return;
        }

        await payment.MarkSucceeded();

        await PublishOutcome(PaymentOutcome.Success, resultPageUrl);
    }

    private async Task FailAsync(Payment payment, string resultPageUrl, PaymentFailureReason? failureReason = null)
    {
        await payment.MarkFailed(failureReason);
        await PublishOutcome(PaymentOutcome.Failure, resultPageUrl);
    }

    private async Task PublishOutcome(PaymentOutcome outcome, string resultPageUrl)
    {
        await Publisher.Publish(new PaymentCallbackProcessedEvent(outcome, resultPageUrl));
    }
}
