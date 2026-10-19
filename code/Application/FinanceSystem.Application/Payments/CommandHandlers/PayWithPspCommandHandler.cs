using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Application.Payments.Gateways;
using FinanceSystem.Domain.Bills.Services;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Services;

namespace FinanceSystem.Application.Payments.CommandHandlers;

public class PayWithPspCommandHandler(
    IPaymentRepository paymentRepository,
    IEventPublisher publisher,
    IEventListener listener,
    TokenEligibilityService tokenEligibilityService,
    PspSelector pspSelector,
    IPspGatewayFactory pspGatewayFactory,
    IUnitOfWork unitOfWork)
    : PaymentCommandHandler<PayWithPspCommand>(paymentRepository, publisher, listener)
{
    public override async Task Handle(PayWithPspCommand command, CancellationToken cancellationToken = default)
    {
        var bill = await tokenEligibilityService.GetBillByToken(command.Token, cancellationToken);

        var psp = await pspSelector.SelectAsync(command.PspCode, cancellationToken);

        var payment = Payment.CreatePspPurchasePayment(
            new Money(bill.Amount),
            bill.FinanceAccountId,
            bill.ServiceId.ToString(),
            bill.ReferenceId.ToString(),
            bill.Id,
            psp.Code,
            Publisher);

        await Repository.AddAsync(payment);

        await unitOfWork.SaveChangesAsync();

        var pspGateway = pspGatewayFactory.Get(psp.Code);

        var pspTokenRequest = new PspTokenRequest()
        {
            Amount = bill.Amount,
            ReservationNumber = payment.Id,
            PaymentPurpose = PaymentPurpose.Purchase
        };

        var tokenResponse = await pspGateway.RequestPaymentTokenAsync(pspTokenRequest, cancellationToken);

        if (tokenResponse.IsSuccess)
        {
            await payment.MarkTokenReceived(tokenResponse.Token, tokenResponse.IpgUrl);
        }
        else
        {
            await payment.MarkTokenRequestFailed(
                PaymentFailureReason.Unknown,
                tokenResponse.Status,
                tokenResponse.StatusCode,
                tokenResponse.Description);
        }
    }
}
