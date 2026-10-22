using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Domain.Bills.Services;
using FinanceSystem.Domain.FinanceAccounts.DomainServices;
using FinanceSystem.Domain.Payments;

namespace FinanceSystem.Application.Payments.CommandHandlers;

public class PayWithWalletCommandHandler(
    IPaymentRepository repository,
    IEventPublisher publisher,
    IEventListener listener,
    TokenEligibilityService tokenEligibilityService,
    FinanceAccountEligibilityService financeAccountEligibilityService) : PaymentCommandHandler<PayWithWalletCommand>(repository, publisher, listener)
{
    public override async Task Handle(PayWithWalletCommand command, CancellationToken cancellationToken = default)
    {
        var bill = await tokenEligibilityService.GetBill(command.Token, command.UserId, cancellationToken);

        var financeAccount = financeAccountEligibilityService.GetEligibleWalletPurchaseAccountAsync(command.UserId, command.FinanceAccountType, cancellationToken);

        // add transaction and reduce the wallet balance

        // raise payment success event
            // send payment result page by event

        throw new NotImplementedException();
    }
}
