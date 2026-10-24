using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Domain.Bills;
using FinanceSystem.Domain.Bills.Services;
using FinanceSystem.Domain.FinanceAccounts.DomainServices;
using FinanceSystem.Domain.LedgerEntries;
using FinanceSystem.Domain.Payments;

namespace FinanceSystem.Application.Bills.CommandHandlers;

public class WalletPaymentCommandHandler(
    IBillRepository repository,
    IEventPublisher publisher,
    IEventListener listener,
    BillEligibilityService billEligibilityService,
    FinanceAccountEligibilityService financeAccountEligibilityService,
    ILedgerEntryRepository ledgerEntryRepository,
    IPaymentRepository paymentRepository)
    : BillCommandHandler<WalletPaymentCommand>(repository, publisher, listener)
{
    public override async Task Handle(WalletPaymentCommand paymentCommand, CancellationToken cancellationToken = default)
    {
        var bill = await billEligibilityService.GetBill(paymentCommand.BillId, paymentCommand.UserId, cancellationToken);

        var financeAccount = await financeAccountEligibilityService.GetEligibleWalletPurchaseAccountAsync(paymentCommand.UserId, paymentCommand.FinanceAccountId, cancellationToken);

        var amount = new Money(bill.Amount);

        var payment = Payment.CreateWalletPurchasePayment(
            amount,
            financeAccount.Id,
            bill.ServiceId.ToString(),
            bill.ReferenceId.ToString(),
            Publisher);

        await paymentRepository.AddAsync(payment);

        var ledgerEntry = LedgerEntry.CreateOutboundEntry(payment.Id, financeAccount.Id, amount);

        await ledgerEntryRepository.AddAsync(ledgerEntry);

        financeAccount.ApplyEntry(ledgerEntry, DateTimeOffset.UtcNow);

        await payment.MarkSucceeded();
    }
}
