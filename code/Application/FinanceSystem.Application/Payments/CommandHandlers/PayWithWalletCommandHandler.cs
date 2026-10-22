using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Domain.Bills.Services;
using FinanceSystem.Domain.FinanceAccounts.DomainServices;
using FinanceSystem.Domain.LedgerEntries;
using FinanceSystem.Domain.Payments;

namespace FinanceSystem.Application.Payments.CommandHandlers;

public class PayWithWalletCommandHandler(
    IPaymentRepository repository,
    IEventPublisher publisher,
    IEventListener listener,
    TokenEligibilityService tokenEligibilityService,
    FinanceAccountEligibilityService financeAccountEligibilityService,
    ILedgerEntryRepository ledgerEntryRepository) : PaymentCommandHandler<PayWithWalletCommand>(repository, publisher, listener)
{
    public override async Task Handle(PayWithWalletCommand command, CancellationToken cancellationToken = default)
    {
        var bill = await tokenEligibilityService.GetBill(command.Token, command.UserId, cancellationToken);

        var financeAccount = await financeAccountEligibilityService.GetEligibleWalletPurchaseAccountAsync(command.UserId, command.FinanceAccountType, cancellationToken);

        var amount = new Money(bill.Amount);

        var payment = Payment.CreateWalletPurchasePayment(
            amount,
            financeAccount.Id,
            bill.ServiceId.ToString(),
            bill.ReferenceId.ToString(),
            Publisher);

        await Repository.AddAsync(payment);

        var ledgerEntry = LedgerEntry.CreateOutboundEntry(payment.Id, financeAccount.Id, amount);

        await ledgerEntryRepository.AddAsync(ledgerEntry);

        financeAccount.ApplyEntry(ledgerEntry, DateTimeOffset.UtcNow);

        await payment.MarkSucceeded();
    }
}
