using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Application.Payments.Gateways;
using FinanceSystem.Domain.RequestsToPay;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Services;
using FinanceSystem.Domain.RequestsToPay.Enums;

namespace FinanceSystem.Application.Payments.CommandHandlers;

public class TopUpCommandHandler(
    IPaymentRepository repository,
    IEventPublisher publisher,
    IEventListener listener,
    IRequestToPayRepository requestToPayRepository,
    PspSelector pspSelector,
    IPspGatewayFactory pspGatewayFactory,
    IUnitOfWork unitOfWork)
    : PaymentCommandHandler<TopUpCommand>(repository, publisher, listener)
{
    public override async Task Handle(TopUpCommand command, CancellationToken cancellationToken = default)
    {
        var targetAccountId = 12;

        var amount = new Money(command.Amount);

        var psp = await pspSelector.SelectAsync((PspCode)command.PspCode, cancellationToken);

        var pspCode = psp.Code;

        var requestToPay = await RequestToPay.Create(
            pspCode,
            targetAccountId,
            amount,
            publisher);

        await requestToPayRepository.AddAsync(requestToPay, cancellationToken);

        await unitOfWork.SaveChangesAsync();

        var pspGateway = pspGatewayFactory.Get(pspCode);

        var pspPaymentRequest = new PspTokenRequest()
        {
            Amount = amount.Value,
            ReferenceNumber = requestToPay.ReferenceNumber,
        };

        var paymentTokenResponse = await pspGateway.RequestTokenAsync(pspPaymentRequest, cancellationToken);

        if (!paymentTokenResponse.IsSuccess)
        {
            await requestToPay.MarkTokenRequestFailed(
                paymentTokenResponse.FailureReason ?? PspFailureReason.Unknown,
                paymentTokenResponse.RawStatus,
                paymentTokenResponse.RawErrorCode,
                paymentTokenResponse.RawDescription);
        }
        else
        {
            await requestToPay.MarkTokenReceived(
                paymentTokenResponse.PaymentToken,
                paymentTokenResponse.IpgUrl);
        }
    }
}
