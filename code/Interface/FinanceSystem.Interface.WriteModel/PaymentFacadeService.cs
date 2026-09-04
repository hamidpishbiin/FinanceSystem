using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Domain.Contract.Payments;
using FinanceSystem.Interface.Contracts.Payments.Models;
using FinanceSystem.Interface.Contracts.Payments.Services;

namespace FinanceSystem.Interface.WriteModel;

public class PaymentFacadeService(
    ICommandBus _commandBus,
    IEventListener _listener) : IPaymentFacadeService
{
    public async Task<JsonResponse<string>> Create(CreatePaymentModel model)
    {
        string idempotencyKey = string.Empty;
        await _listener.Subscribe(new ActionEventHandler<PaymentCreatedEvent>(a =>
        {
            idempotencyKey = a.IdempotencyKey;
        }));

        await _commandBus.Dispatch(new CreatePaymentCommand
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
            BankPaymentDetailId = model.BankPaymentDetailId
        });

        return JsonResponse<string>.Success(idempotencyKey);
    }
}
