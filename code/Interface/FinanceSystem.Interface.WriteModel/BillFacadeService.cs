using FinanceSystem.Application.Contracts.Bills.Commands;
using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Application.Contracts.Payments.Events;
using FinanceSystem.Domain.Contract.Bills;
using FinanceSystem.Domain.Contract.Payments;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Exceptions;
using FinanceSystem.Interface.Contracts.Bills.Models;
using FinanceSystem.Interface.Contracts.Bills.Services;
using FinanceSystem.Interface.Contracts.Payments.Models;
using Shared.Core.Exceptions;

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

    public async Task<JsonResponse<PspHandoffModel>> PayWithPspAsync(PspPaymentRequest request, long userId, CancellationToken cancellationToken = default)
    {
        PspHandoffModel? handoff = null;

        await eventListener.Subscribe(new EventHandlerAction<PspHandoffReadyEvent>(e =>
        {
            handoff = new PspHandoffModel
            {
                Url = e.Handoff.Url,
                Method = e.Handoff.Method,
                Fields = e.Handoff.Fields
            };
        }));

        var pspCode = (PspCode)request.PspCodeValue;

        Guard<InvalidPspCodeException>.IsTrue(!Enum.IsDefined(pspCode));

        await commandBus.Dispatch(new PspPaymentCommand()
        {
            BillId = request.BillId,
            UserId = userId,
            PspCode = pspCode
        }, cancellationToken);

        return handoff is null
            ? JsonResponse<PspHandoffModel>.Failure("Payment gateway token request failed. Please try again.")
            : JsonResponse<PspHandoffModel>.Success(handoff);
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
