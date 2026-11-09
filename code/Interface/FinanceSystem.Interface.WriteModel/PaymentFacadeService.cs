using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Application.Contracts.Payments.Events;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Exceptions;
using FinanceSystem.Interface.Contracts.Payments.Services;
using Shared.Core.Exceptions;

namespace FinanceSystem.Interface.WriteModel;

public class PaymentFacadeService(
    ICommandBus commandBus,
    IEventListener listener) : IPaymentFacadeService
{
    public async Task<JsonResponse<string>> ProcessPaymentCallbackAsync(string psp, Dictionary<string, string> fields, CancellationToken cancellationToken)
    {
        Guard<InvalidPspCodeException>.IsTrue(!Enum.TryParse<PspCode>(psp, ignoreCase: true, out var pspCode) || !Enum.IsDefined(pspCode));

        var resultPageUrl = string.Empty;
        var isSuccess = false;

        await listener.Subscribe(new EventHandlerAction<PaymentCallbackProcessed>(e =>
        {
            isSuccess = e.IsSuccess;
            resultPageUrl = e.ResultPageUrl;
        }));

        try
        {
            await commandBus.Dispatch(new ProcessPaymentCallbackCommand()
                {
                    PspCode = pspCode,
                    Fields = fields
                },
                cancellationToken);
        }
        catch (BusinessException)
        {
            // ignored
        }

        return isSuccess
            ? JsonResponse<string>.Success(resultPageUrl)
            : JsonResponse<string>.Failure(resultPageUrl);
    }
}
