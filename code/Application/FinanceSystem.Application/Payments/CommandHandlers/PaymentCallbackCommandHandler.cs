using FinanceSystem.Application.Contracts.Gateways;
using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Application.Contracts.Payments.Events;
using FinanceSystem.Application.Gateways;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using FinanceSystem.Domain.PspExchangeLogs;
using Microsoft.Extensions.Options;

namespace FinanceSystem.Application.Payments.CommandHandlers;

public class PaymentCallbackCommandHandler(
    IPaymentRepository repository,
    IPspGatewayFactory pspGatewayFactory,
    IEventPublisher publisher,
    IEventListener listener,
    IOptions<PspOptions> pspOptions,
    PaymentEligibilityService paymentEligibilityService,
    IPspExchangeLogRepository pspExchangeLogRepository)
    : PaymentCommandHandler<ProcessPaymentCallbackCommand>(repository, publisher, listener)
{
    public override async Task Handle(ProcessPaymentCallbackCommand command, CancellationToken cancellationToken = default)
    {
        var resultPageUrl = string.Empty;
        Payment payment = null;
        var failureReason = PaymentFailureReason.CallbackFailed;

        try
        {
            var pspGateway = pspGatewayFactory.Get(command.PspCode);

            resultPageUrl = pspOptions.Value.For(pspGateway.Code).PaymentResultPageUrl;

            var callback = pspGateway.ParseCallback(command.Fields);

            var callbackLog = PspExchangeLog.CreateCallbackLog(callback.ReservationNumber, command.Fields.ToString());

            pspExchangeLogRepository.Add(callbackLog);

            payment = await paymentEligibilityService.GetPurchasePaymentAsync(callback.ReservationNumber, cancellationToken);

            payment.EnsureCallbackValidity(command.PspCode, callback.Token, callback.Amount);

            payment.Publisher = Publisher;

            if (payment.State == PaymentState.Succeeded)
            {
                await PublishOutcome(isSuccess: true, resultPageUrl);
                return;
            }

            await paymentEligibilityService.ValidateReferenceNumberAsync(callback.ReferenceNumber, cancellationToken);

            payment.StoreCallback(
                callback.ReferenceNumber,
                callback.Amount,
                callback.MaskedPan,
                callback.RRN,
                callback.TraceNumber);

            if (!callback.IsSuccess)
            {
                await FailAsync(payment, resultPageUrl, failureReason);
                return;
            }

            payment.MarkVerifying();

            var verificationRequest = new PaymentVerificationRequest()
            {
                ReservationNumber = payment.Id,
                ReferenceNumber = payment.ReferenceNumber!,
                Amount = payment.Amount.Value
            };

            var verification = await pspGateway.VerifyPaymentAsync(verificationRequest, cancellationToken);

            if (!verification.IsSuccess)
            {
                failureReason = PaymentFailureReason.VerificationFailed;

                await FailAsync(payment, resultPageUrl, failureReason);
                return;
            }

            await payment.MarkSucceeded();

            await PublishOutcome(isSuccess: true, resultPageUrl);
        }
        catch
        {
            await FailAsync(payment!, resultPageUrl, failureReason);
            throw;
        }
    }

    private async Task FailAsync(Payment payment, string resultPageUrl, PaymentFailureReason failureReason)
    {
        await payment.MarkFailed(failureReason);
        await PublishOutcome(isSuccess: false, resultPageUrl);
    }

    private async Task PublishOutcome(bool isSuccess, string resultPageUrl)
    {
        var status = isSuccess
            ? "Success"
            : "Failure";

        await Publisher.Publish(new PaymentCallbackProcessed(isSuccess, $"{resultPageUrl}?status={status}"));
    }
}
