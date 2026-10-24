using FinanceSystem.Application.Contracts.Bills.Commands;
using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Domain.Contract.Bills;
using FinanceSystem.Domain.Contract.Payments;
using FinanceSystem.Interface.Contracts.Bills.Models;
using FinanceSystem.Interface.Contracts.Bills.Services;

namespace FinanceSystem.Interface.WriteModel;

public class BillFacadeService(ICommandBus commandBus, IEventListener eventListener) : IBillFacadeService
{
    public async Task<JsonResponse<long>> CreateBill(CreateBillRequest request, CancellationToken cancellationToken)
    {
        long billId = 0;

        await eventListener.Subscribe(new EventHandlerAction<BillCreatedEvent>(bce =>
        {
            billId = bce.BillId;
        }));

        await commandBus.Dispatch(new CreateBillCommand()
        {
            ServiceId = request.ServiceId,
            ReferenceId = request.ReferenceId,
            Amount = request.Amount,
            UserId = request.UserId
        }, cancellationToken);

        return JsonResponse<long>.Success(billId);
    }

    public async Task<JsonResponse<string>> PayWithPspAsync(PspPaymentRequest request, long userId, CancellationToken cancellationToken = default)
    {
        var ipgUrl = string.Empty;

        await eventListener.Subscribe(new EventHandlerAction<PspTokenReceivedEvent>(e =>
        {
            ipgUrl = e.IpgUrl;
        }));

        await commandBus.Dispatch(new PspPaymentCommand()
        {
            Token = long.Parse(request.BillId),
            UserId = userId,
            PspCode = request.PspCode
        }, cancellationToken);

        return JsonResponse<string>.Success(ipgUrl);
    }

    public async Task<JsonResponse<string>> PayWithWalletAsync(long billId, long userId, long financeAccountId, CancellationToken cancellationToken)
    {
        var paymentSucceeded = false;
        var referenceId = string.Empty;

        await eventListener.Subscribe(new EventHandlerAction<PaymentSucceededEvent>(e =>
        {
            paymentSucceeded = true;
            referenceId = e.ReferenceId;
        }));

        await commandBus.Dispatch(new WalletPaymentCommand()
        {
            BillId = billId,
            FinanceAccountId = financeAccountId,
            UserId = userId
        }, cancellationToken);

        return paymentSucceeded
            ? JsonResponse<string>.Success(referenceId)
            : JsonResponse<string>.Failure(referenceId);
    }
}
