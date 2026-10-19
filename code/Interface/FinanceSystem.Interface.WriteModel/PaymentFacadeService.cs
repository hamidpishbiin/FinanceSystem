using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Application.Contracts.Payments.Events;
using FinanceSystem.Domain.Contract.Payments;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Interface.Contracts.Payments.Models;
using FinanceSystem.Interface.Contracts.Payments.Services;

namespace FinanceSystem.Interface.WriteModel;

public class PaymentFacadeService(ICommandBus commandBus, IEventListener listener) : IPaymentFacadeService
{
    public async Task<JsonResponse<string>> RequestPaymentTokenAsync(PspPaymentModel model, CancellationToken cancellationToken = default)
    {
        var ipgUrl = string.Empty;

        await listener.Subscribe(new EventHandlerAction<PspTokenReceivedEvent>(e =>
        {
            ipgUrl = e.IpgUrl;
        }));

        await commandBus.Dispatch(new PayWithPspCommand()
        {
            Token = long.Parse(model.Token),
            PspCode = model.PspCode
        }, cancellationToken);

        return JsonResponse<string>.Success(ipgUrl);
    }

    public async Task<JsonResponse<string>> CompletePaymentProcessAsync(string psp, Dictionary<string, string> fields, CancellationToken cancellationToken)
    {
        var outcome = PaymentOutcome.Failure;

        if (!Enum.TryParse<PspCode>(psp, ignoreCase: true, out var pspCode) || !Enum.IsDefined(pspCode))
        {
            return JsonResponse<string>.Failure(outcome.ToString().ToLowerInvariant());
        }

        var resultPageUrl = string.Empty;

        await listener.Subscribe(new EventHandlerAction<PaymentCallbackHandledEvent>(e =>
        {
            outcome = e.Outcome;
            resultPageUrl = e.ResultPageUrl;
        }));

        await commandBus.Dispatch(new PaymentCallbackCommand()
        {
            PspCode = pspCode,
            Fields = fields
        }, cancellationToken);

        return JsonResponse<string>.Success($"{resultPageUrl}?status={outcome.ToString().ToLowerInvariant()}");
    }
}
