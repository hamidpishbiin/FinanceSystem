using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Application.Payments.Gateways;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Services;

namespace FinanceSystem.Application.Payments.CommandHandlers;

public class TopUpCommandHandler(
    IPaymentRepository repository,
    IEventPublisher publisher,
    IEventListener listener,
    PspSelector pspSelector,
    IPspGatewayFactory pspGatewayFactory,
    IGuidGenerator guidGenerator,
    IUnitOfWork unitOfWork)
    : PaymentCommandHandler<TopUpCommand>(repository, publisher, listener)
{
    private const string OriginServiceId = "FinanceSystem";
    private const string ExternalTag = "TopUp";

    public override async Task Handle(TopUpCommand command, CancellationToken cancellationToken = default)
    {
        var sourceFinanceAccountId = 1;
        var targetFinanceAccountId = 12;

        var amount = new Money(command.Amount);

        var psp = await pspSelector.SelectAsync((PspCode)command.PspCode, cancellationToken);

        var pspCode = psp.Code;

        var payment = await Payment.Create(
            PaymentPurpose.TopUp,
            PaymentChannel.Psp,
            amount,
            sourceFinanceAccountId,
            targetFinanceAccountId,
            OriginServiceId,
            guidGenerator.New().ToString(),
            ExternalTag,
            pspCode,
            Publisher);

        await Repository.AddAsync(payment);

        await unitOfWork.SaveChangesAsync();

        var pspGateway = pspGatewayFactory.Get(pspCode);

        var pspPaymentRequest = new PspTokenRequest()
        {
            Amount = amount.Value,
            ReferenceNumber = payment.Id,
        };

        var paymentTokenResponse = await pspGateway.RequestTokenAsync(pspPaymentRequest, cancellationToken);

        if (!paymentTokenResponse.IsSuccess)
        {
            await payment.MarkTokenRequestFailed(
                paymentTokenResponse.FailureReason ?? PspFailureReason.Unknown,
                paymentTokenResponse.RawStatus,
                paymentTokenResponse.RawErrorCode,
                paymentTokenResponse.RawDescription);
        }
        else
        {
            await payment.MarkTokenReceived(
                paymentTokenResponse.PaymentToken,
                paymentTokenResponse.IpgUrl);
        }
    }
}
