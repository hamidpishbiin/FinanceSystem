using FinanceSystem.Application.Contracts.Bills.Commands;
using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Domain.Contract.Bills;
using FinanceSystem.Interface.Contracts.Bills.Models;
using FinanceSystem.Interface.Contracts.Bills.Services;

namespace FinanceSystem.Interface.WriteModel;

public class BillFacadeService(ICommandBus commandBus, IEventListener eventListener) : IBillFacadeService
{
    public async Task<JsonResponse<long>> CreateBill(CreateBillModel model, string userId, CancellationToken cancellationToken)
    {
        long billId = 0;

        await eventListener.Subscribe(new EventHandlerAction<BillCreatedEvent>(bce =>
        {
            billId = bce.BillId;
        }));

        await commandBus.Dispatch(new CreateBillCommand()
        {
            ServiceId = model.ServiceId,
            ReferenceId = model.ReferenceId,
            Amount = model.Amount,
            UserId = long.Parse(userId)
        }, cancellationToken);

        return JsonResponse<long>.Success(billId);
    }
}
