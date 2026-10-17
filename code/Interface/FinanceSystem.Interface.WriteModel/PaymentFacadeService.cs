using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Domain.Contract.Payments;
using FinanceSystem.Interface.Contracts.Payments.Models;
using FinanceSystem.Interface.Contracts.Payments.Services;

namespace FinanceSystem.Interface.WriteModel;

public class PaymentFacadeService(ICommandBus commandBus, IEventListener listener) : IPaymentFacadeService
{
    public async Task<JsonResponse<string>> PayWithPspAsync(PspPaymentModel model, CancellationToken cancellationToken = default)
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
}
