using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Application.Payments.Gateways;
using FinanceSystem.Domain.Bills;
using FinanceSystem.Domain.Bills.Services;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Services;

namespace FinanceSystem.Application.Bills.CommandHandlers;

public class PspPaymentCommandHandler(
    IBillRepository billRepository,
    IEventPublisher publisher,
    IEventListener listener,
    BillEligibilityService billEligibilityService,
    PspSelector pspSelector,
    IPspGatewayFactory pspGatewayFactory,
    IUnitOfWork unitOfWork,
    IPaymentRepository paymentRepository)
    : BillCommandHandler<PspPaymentCommand>(billRepository, publisher, listener)
{
    public override async Task Handle(PspPaymentCommand paymentCommand, CancellationToken cancellationToken = default)
    {
        var bill = await billEligibilityService.GetBill(paymentCommand.Token, paymentCommand.UserId, cancellationToken);

        var psp = await pspSelector.SelectAsync(paymentCommand.PspCode, cancellationToken);

        var payment = Payment.CreatePspPurchasePayment(
            new Money(bill.Amount),
            bill.FinanceAccountId,
            bill.ServiceId.ToString(),
            bill.ReferenceId.ToString(),
            bill.Id,
            psp.Code,
            Publisher);

        await paymentRepository.AddAsync(payment);

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
