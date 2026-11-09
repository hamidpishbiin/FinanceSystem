using FinanceSystem.Application.Contracts.Gateways;
using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Application.Contracts.Payments.Events;
using FinanceSystem.Application.Gateways;
using FinanceSystem.Domain.LedgerEntries;
using FinanceSystem.Domain.FinanceAccounts;
using FinanceSystem.Domain.FinanceAccounts.DomainServices;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using FinanceSystem.Domain.Payments.Exceptions;
using FinanceSystem.Domain.PspExchangeLogs;
using Microsoft.Extensions.Options;

namespace FinanceSystem.Application.Payments.CommandHandlers;

public class TopUpCallbackCommandHandler(
    IPaymentRepository repository,
    FinanceAccountEligibilityService financeAccountEligibilityService,
    ILedgerEntryRepository ledgerEntryRepository,
    IPspGatewayFactory pspGatewayFactory,
    IUnitOfWork unitOfWork,
    IEventPublisher publisher,
    IEventListener listener,
    IOptions<PspOptions> pspOptions) : PaymentCommandHandler<TopUpCallbackCommand>(repository, publisher, listener)
{
    public override async Task Handle(TopUpCallbackCommand command, CancellationToken cancellationToken = default)
    {
        var pspGateway = pspGatewayFactory.Get(command.PspCode);

        var resultPageUrl = pspOptions.Value.For(pspGateway.Code).TopUpResultPageUrl;

        var callback = pspGateway.ParseCallback(command.Fields);

        var payment = await Repository.GetByIdAsync(callback.ReservationNumber, cancellationToken);

        Guard<PaymentNotFoundException>.AgainstNull(payment);

        PspExchangeLog.CreateCallbackLog(payment!.Id, command.Fields.ToString());

        if (payment.State == PaymentState.Succeeded)
        {
            await PublishOutcome(TopUpOutcome.Success, resultPageUrl);
            return;
        }

        var isReferenceNumberUsed = callback.IsSuccess && await Repository.GetByReferenceNumberAsync(callback.ReferenceNumber, cancellationToken) is not null;

        Guard<DuplicateReferenceNumberException>.IsTrue(isReferenceNumberUsed);

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

        var userAccount = await financeAccountEligibilityService.GetEligibleAccountForTopUpAsync(payment.UserId, cancellationToken);

        payment.MarkVerifying();

        await unitOfWork.SaveChangesAsync();

        var transactionRequest = new PaymentVerificationRequest()
        {
            ReservationNumber = payment.Id,
            ReferenceNumber = payment.ReferenceNumber!,
            Amount = payment.Amount.Value
        };

        var verification = await pspGateway.VerifyPaymentAsync(transactionRequest, cancellationToken);

        if (!verification.IsSuccess)
        {
            await FailAsync(payment, resultPageUrl, PaymentFailureReason.VerificationFailed);
            return;
        }

        await payment.MarkSucceeded(Publisher);

        await CreditWalletAsync(payment, userAccount);

        await PublishOutcome(TopUpOutcome.Success, resultPageUrl);
    }

    private async Task CreditWalletAsync(Payment payment, FinanceAccount userAccount)
    {
        var amount = payment.Amount;
        var occurredAt = DateTimeOffset.UtcNow;

        var userEntry = LedgerEntry.CreateInboundEntry(payment.Id, userAccount.Id, amount);

        userAccount.ApplyEntry(userEntry, occurredAt);

        await ledgerEntryRepository.AddAsync(userEntry);
    }

    private async Task FailAsync(Payment payment, string resultPageUrl, PaymentFailureReason? failureReason = null)
    {
        await payment.MarkFailed(failureReason, Publisher);
        await PublishOutcome(TopUpOutcome.Failure, resultPageUrl);
    }

    private async Task PublishOutcome(TopUpOutcome outcome, string resultPageUrl)
    {
        await Publisher.Publish(new TopUpCallbackHandled(outcome, resultPageUrl));
    }
}
