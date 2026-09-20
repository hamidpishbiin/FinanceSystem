using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Application.Payments.Gateways;
using FinanceSystem.Domain.PspPaymentDetails;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using FinanceSystem.Domain.PaymentServiceProviders;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Services;
using FinanceSystem.Domain.PspPaymentDetails.Enums;
using Microsoft.Extensions.Options;

namespace FinanceSystem.Application.Payments.CommandHandlers;

public class TopUpPaymentCommandHandler(
    IPaymentRepository repository,
    IEventPublisher publisher,
    IEventListener listener,
    IPspPaymentDetailRepository pspPaymentDetailRepository,
    PspSelector pspSelector,
    IPspGatewayFactory pspGatewayFactory,
    IOptions<PspCallbackUrlOptions> callbackUrls,
    IUnitOfWork unitOfWork)
    : PaymentCommandHandler<TopUpPaymentCommand>(repository, publisher, listener)
{
    public override async Task Handle(TopUpPaymentCommand command, CancellationToken cancellationToken = default)
    {
        var targetAccountId = 12;

        var amount = new Money(command.AmountRial);

        var psp = await pspSelector.SelectAsync((PspCode)command.PspCode, cancellationToken);

        var pspCode = psp.Code;

        var pspPaymentDetail = await PspPaymentDetail.Create(
            pspCode,
            targetAccountId,
            amount,
            publisher);

        await pspPaymentDetailRepository.AddAsync(pspPaymentDetail, cancellationToken);

        await unitOfWork.SaveChangesAsync();

        var pspGateway = pspGatewayFactory.Get(pspCode);

        var pspPaymentRequest = new PspPaymentRequest()
        {
            Amount = amount.Value,
            ReferenceNumber = pspPaymentDetail.ReferenceNumber,
            CallbackUrl = callbackUrls.Value.TopUp
        };

        var paymentTokenResponse = await pspGateway.RequestPaymentTokenAsync(pspPaymentRequest, cancellationToken);

        if (!paymentTokenResponse.IsSuccess)
        {
            await pspPaymentDetail.MarkPaymentFailed();
        }
        else
        {
            await pspPaymentDetail.MarkPaymentTokenReceived(
                paymentTokenResponse.PaymentToken,
                paymentTokenResponse.IpgUrl);
        }
    }
}
