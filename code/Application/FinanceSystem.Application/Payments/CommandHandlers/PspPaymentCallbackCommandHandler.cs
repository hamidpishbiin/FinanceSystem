using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Application.Contracts.Payments.Events;
using FinanceSystem.Application.Payments.Gateways;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using FinanceSystem.Domain.Payments.Exceptions;
using Microsoft.Extensions.Options;

namespace FinanceSystem.Application.Payments.CommandHandlers;

public class PspPaymentCallbackCommandHandler(
    IPaymentRepository repository,
    IPspGatewayFactory pspGatewayFactory,
    IUnitOfWork unitOfWork,
    IEventPublisher publisher,
    IEventListener listener,
    IOptions<PspOptions> pspOptions)
    : PaymentCommandHandler<ProcessPspPaymentCallbackCommand>(repository, publisher, listener)
{
    public override async Task Handle(ProcessPspPaymentCallbackCommand command, CancellationToken cancellationToken = default)
    {
        var pspGateway = pspGatewayFactory.Get(command.PspCode);

        var resultPageUrl = pspOptions.Value.For(pspGateway.Code).PaymentResultPageUrl; // Should it be here?

        var callback = pspGateway.ParseCallback(command.Fields);

        var payment = await Repository.GetByIdAsync(callback.ReservationNumber, cancellationToken); // Should be done by a domain service

        Guard<PaymentNotFoundException>.AgainstNull(payment);

        Guard<InvalidPaymentPurposeException>.IsTrue(payment!.Purpose != PaymentPurpose.Purchase);

        payment.Publisher = Publisher;

        if (payment.State == PaymentState.Succeeded)
        {
            await PublishOutcome(PaymentOutcome.Success, resultPageUrl);
            return;
        }

        var isReferenceNumberUsed = callback.IsSuccess && await Repository.GetByReferenceNumberAsync(callback.ReferenceNumber, cancellationToken) is not null; // Domain service

        Guard<DuplicateReferenceNumberException>.IsTrue(isReferenceNumberUsed);

        Guard<PspCodeMismatchException>.IsTrue(payment.PspCode != pspGateway.Code);
        Guard<CallbackTokenMismatchException>.IsTrue(payment.Token != callback.Token);

        Guard<InvalidPaymentStateException>.IsTrue(payment.State != PaymentState.TokenReceived);

        payment.StoreCallback(
            callback.Status,
            callback.StatusCode,
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
