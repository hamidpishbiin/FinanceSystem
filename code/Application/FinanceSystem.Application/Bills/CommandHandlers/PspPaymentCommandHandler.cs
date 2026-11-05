using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Application.Gateways;
using FinanceSystem.Domain.Bills;
using FinanceSystem.Domain.Bills.Services;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Services;
using Microsoft.Extensions.Options;

namespace FinanceSystem.Application.Bills.CommandHandlers;

public class PspPaymentCommandHandler(
    IBillRepository billRepository,
    IEventPublisher publisher,
    IEventListener listener,
    BillEligibilityService billEligibilityService,
    PspSelector pspSelector,
    IPspGatewayFactory pspGatewayFactory,
    IPaymentRepository paymentRepository,
    IOptions<PspOptions> pspOptions)
    : BillCommandHandler<PspPaymentCommand>(billRepository, publisher, listener)
{
    public override async Task Handle(PspPaymentCommand paymentCommand, CancellationToken cancellationToken = default)
    {
        var bill = await billEligibilityService.GetBill(paymentCommand.BillId, paymentCommand.UserId, cancellationToken);

        var pspEnum = (PspCode)paymentCommand.PspCode;

        var psp = await pspSelector.SelectAsync(pspEnum, cancellationToken);

        var payment = Payment.CreatePspPurchasePayment(
            new Money(bill.Amount),
            bill.UserId,
            bill.ServiceId.ToString(),
            bill.ReferenceId.ToString(),
            bill.Id,
            psp.Code,
            Publisher);

        await paymentRepository.AddAsync(payment);

        var pspGateway = pspGatewayFactory.Get(psp.Code);

        var request = new PspTokenRequest()
        {
            Amount = bill.Amount,
            ReservationNumber = payment.Id,
            PaymentPurpose = PaymentPurpose.Purchase
        };

        var response = await pspGateway.RequestPaymentTokenAsync(request, cancellationToken);

        if (response.IsSuccess)
        {
            await payment.MarkTokenReceived(response.Token, GetPspRedirectUrl(pspEnum, response.Token));
        }
        else
        {
            await payment.MarkTokenRequestFailed(
                PaymentFailureReason.Unknown,
                response.Status.ToString(),
                response.StatusCode,
                response.Description);
        }
    }

    private string GetPspRedirectUrl(PspCode pspCode, string token)
    {
        return $"{pspOptions.Value.For(pspCode).BaseUrl}/OnlinePG/SendToken?token={token}";
    }
}
