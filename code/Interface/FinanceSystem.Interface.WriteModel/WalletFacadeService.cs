using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Domain.Contract.Payments;
using FinanceSystem.Domain.Contract.RequestsToPay;
using FinanceSystem.Interface.Contracts.Payments.Models;
using FinanceSystem.Interface.Contracts.Payments.Services;
using Shared.Core;

namespace FinanceSystem.Interface.WriteModel;

public class WalletFacadeService(
    ICommandBus commandBus,
    IEventListener listener,
    IUserResolver userResolver) : IPaymentFacadeService
{
    public async Task<JsonResponse<string>> Create(CreatePaymentModel model)
    {
        string idempotencyKey = string.Empty;
        await listener.Subscribe(new EventHandlerAction<PaymentCreatedEvent>(a =>
        {
            idempotencyKey = a.IdempotencyKey;
        }));

        await commandBus.Dispatch(new CreatePaymentCommand
        {
            IdempotencyKey = model.IdempotencyKey,
            Purpose = model.Purpose,
            Channel = model.Channel,
            AmountRial = model.AmountRial,
            SourceAccountId = model.SourceAccountId,
            DestinationAccountId = model.DestinationAccountId,
            OriginServiceId = model.OriginServiceId,
            ExternalReferenceId = model.ExternalReferenceId,
            ExternalTag = model.ExternalTag,
            RequestToPayId = model.RequestToPayId
        });

        return JsonResponse<string>.Success(idempotencyKey);
    }

    public async Task<JsonResponse<string>> TopUpAsync(TopUpModel model, CancellationToken cancellationToken = default)
    {
        var ipgUrl = string.Empty;

        await listener.Subscribe(new EventHandlerAction<RequestToPayTokenReceivedEvent>(a =>
        {
            ipgUrl = a.IpgUrl;
        }));

        await commandBus.Dispatch(new TopUpCommand()
        {
            AmountRial = model.Amount,
            PspCode = model.PspCode,
            UserId = Guid.Parse(userResolver.GetUserId())
        }, cancellationToken);

        return JsonResponse<string>.Success(ipgUrl);
    }
}
