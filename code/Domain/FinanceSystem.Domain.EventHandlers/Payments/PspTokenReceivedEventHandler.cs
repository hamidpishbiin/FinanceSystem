using FinanceSystem.Domain.Contract;
using FinanceSystem.Domain.Contract.Payments;

namespace FinanceSystem.Domain.EventHandlers.Payments;

public class PspTokenReceivedEventHandler(IDomainEventRepository domainEventRepository)
    : PaymentEventHandlerBase<PspTokenReceived>(domainEventRepository)
{
}
