using FinanceSystem.Application.Contracts.Gateways;
using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Application.Contracts.Payments.Events;
using FinanceSystem.Application.Gateways;
using FinanceSystem.Domain.Bills;
using FinanceSystem.Domain.Bills.Services;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
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
    public override async Task Handle(PspPaymentCommand command, CancellationToken cancellationToken = default)
    {
        var bill = await billEligibilityService.GetBill(command.BillId, command.UserId, cancellationToken);

        var psp = await pspSelector.SelectAsync(command.PspCode, cancellationToken);

        var payment = Payment.CreatePspPurchasePayment(
            bill.Amount,
            bill.UserId,
            bill.ServiceId.ToString(),
            bill.ReferenceId.ToString(),
            bill.Id,
            psp.Code);

        await paymentRepository.AddAsync(payment);

        var pspGateway = pspGatewayFactory.Get(psp.Code);

        var request = new PspTokenRequest()
        {
            Amount = bill.Amount.Value,
            ReservationNumber = payment.Id,
            CallbackUrl = pspOptions.Value.For(psp.Code).PaymentCallBackUrl
        };

        var response = await pspGateway.RequestPaymentTokenAsync(request, cancellationToken);

        if (response.IsSuccess)
        {
            await payment.MarkTokenReceived(response.Token, Publisher);
            await Publisher.Publish(new PspHandoffReady(response.Handoff!));
        }
        else
        {
            await payment.MarkTokenRequestFailed(PaymentFailureReason.TokenRequestFailed, Publisher);
        }
    }
}
