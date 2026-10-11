using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Application.Contracts.Payments.Events;
using FinanceSystem.Application.Payments.Gateways;
using FinanceSystem.Domain.AccountEntries;
using FinanceSystem.Domain.AccountEntries.Enums;
using FinanceSystem.Domain.FinanceAccounts;
using FinanceSystem.Domain.FinanceAccounts.Exceptions;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using FinanceSystem.Domain.Payments.Exceptions;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;

namespace FinanceSystem.Application.Payments.CommandHandlers;

public class CompleteTopUpProcessCommandHandler(
    IPaymentRepository repository,
    IFinanceAccountRepository financeAccountRepository,
    IAccountEntryRepository accountEntryRepository,
    IPspGatewayFactory pspGatewayFactory,
    IUnitOfWork unitOfWork,
    IEventPublisher publisher,
    IEventListener listener) : PaymentCommandHandler<CompleteTopUpProcessCommand>(repository, publisher, listener)
{
    public override async Task Handle(CompleteTopUpProcessCommand command, CancellationToken cancellationToken = default)
    {
        var pspGateway = pspGatewayFactory.Get((PspCode)command.PspCode);

        var callback = pspGateway.ParseCallback(command.Fields);

        var payment = await Repository.GetByIdAsync(callback.ReservationNumber, cancellationToken);

        Guard<PaymentNotFoundException>.AgainstNull(payment);

        payment!.Publisher = Publisher;

        Guard<PspCodeMismatchException>.IsTrue(payment.PspCode != pspGateway.Code);
        Guard<CallbackTokenMismatchException>.IsTrue(payment.Token != callback.Token);

        if (payment.State == PaymentState.Succeeded)
        {
            await PublishOutcome(TopUpOutcome.Success);
            return;
        }

        Guard<InvalidPaymentStateException>.IsTrue(payment.State != PaymentState.TokenReceived);

        var isReferenceNumberUsed = callback.IsSuccess
            && !string.IsNullOrWhiteSpace(callback.ReferenceNumber)
            && await Repository.GetByReferenceNumberAsync(callback.ReferenceNumber, cancellationToken) is not null;

        payment.StoreCallback(
            callback.RawStatus,
            callback.RawStatusCode,
            isReferenceNumberUsed ? null : callback.ReferenceNumber,
            callback.Amount,
            callback.MaskedPan,
            callback.RRN,
            callback.TraceNumber);

        if (!callback.IsSuccess)
        {
            await FailAsync(payment, callback.FailureReason ?? PspFailureReason.Unknown);
            return;
        }

        if (callback.Amount != payment.Amount)
        {
            await FailAsync(payment, PspFailureReason.AmountMismatch);
            return;
        }

        if (isReferenceNumberUsed)
        {
            await FailAsync(payment, PspFailureReason.DuplicateReference);
            return;
        }

        payment.MarkVerifying();

        await unitOfWork.SaveChangesAsync();

        var transactionRequest = new PaymentVerificationRequest()
        {
            ReservationNumber = payment.Id,
            ReferenceNumber = payment.ReferenceNumber!,
            Amount = payment.Amount
        };

        var verification = await pspGateway.VerifyPaymentAsync(transactionRequest, cancellationToken);

        if (!verification.HasResponse)
        {
            await PublishOutcome(TopUpOutcome.Pending);
            return;
        }

        if (!verification.IsSuccess)
        {
            await FailAsync(payment, PspFailureReason.VerificationFailed);
            return;
        }

        if (verification.Amount != payment.Amount)
        {
            var reversal = await pspGateway.ReversePaymentAsync(transactionRequest, cancellationToken);

            if (!reversal.IsSuccess)
            {
                await PublishOutcome(TopUpOutcome.Pending);
                return;
            }

            await payment.MarkReversed();
            await PublishOutcome(TopUpOutcome.Failure);
            return;
        }

        await payment.MarkSucceeded();

        await CreditWalletAsync(payment, cancellationToken);

        await PublishOutcome(TopUpOutcome.Success);
    }

    private async Task CreditWalletAsync(Payment payment, CancellationToken cancellationToken)
    {
        var amount = new Money(payment.Amount);
        var occurredAt = DateTimeOffset.UtcNow;

        var userAccount = await financeAccountRepository.GetByIdAsync(payment.DestinationFinanceAccountId, cancellationToken);
        var companyWallet = await financeAccountRepository.GetByIdAsync(payment.SourceFinanceAccountId, cancellationToken);

        Guard<FinanceAccountNotFoundException>.AgainstNull(userAccount);
        Guard<SystemFinanceAccountNotFoundException>.AgainstNull(companyWallet);

        var userEntry = new AccountEntry(payment.Id, userAccount!.Id, amount, EntryDirection.In);
        var companyEntry = new AccountEntry(payment.Id, companyWallet!.Id, amount, EntryDirection.Out);

        userAccount.ApplyEntry(userEntry, occurredAt);
        companyWallet.ApplyEntry(companyEntry, occurredAt);

        await accountEntryRepository.AddAsync(userEntry);
        await accountEntryRepository.AddAsync(companyEntry);
    }

    private async Task FailAsync(Payment payment, PspFailureReason failureReason)
    {
        await payment.MarkFailed(failureReason);
        await PublishOutcome(TopUpOutcome.Failure);
    }

    private async Task PublishOutcome(TopUpOutcome outcome)
    {
        await Publisher.Publish(new TopUpCallbackHandledEvent(outcome));
    }
}
