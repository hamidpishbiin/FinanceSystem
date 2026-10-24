using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Application.Contracts.Payments.Events;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Interface.Contracts.Payments.Services;

namespace FinanceSystem.Interface.WriteModel;

public class PaymentFacadeService(ICommandBus commandBus, IEventListener listener) : IPaymentFacadeService
{
    public async Task<JsonResponse<string>> ProcessPaymentCallbackAsync(string psp, Dictionary<string, string> fields, CancellationToken cancellationToken)
    {
        var outcome = PaymentOutcome.Failure;

        if (!Enum.TryParse<PspCode>(psp, ignoreCase: true, out var pspCode) || !Enum.IsDefined(pspCode))
        {
            return JsonResponse<string>.Failure(outcome.ToString().ToLowerInvariant());
        }

        var resultPageUrl = string.Empty;

        await listener.Subscribe(new EventHandlerAction<PaymentCallbackProcessedEvent>(e =>
        {
            outcome = e.Outcome;
            resultPageUrl = e.ResultPageUrl;
        }));

        await commandBus.Dispatch(new ProcessPspPaymentCallbackCommand()
        {
            PspCode = pspCode,
            Fields = fields
        }, cancellationToken);

        return JsonResponse<string>.Success($"{resultPageUrl}?status={outcome.ToString().ToLowerInvariant()}");
    }
}
