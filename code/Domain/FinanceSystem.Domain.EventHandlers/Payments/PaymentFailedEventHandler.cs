using FinanceSystem.Domain.Contract;
using FinanceSystem.Domain.Contract.Payments;

namespace FinanceSystem.Domain.EventHandlers.Payments;

public class PaymentFailedEventHandler(IDomainEventRepository domainEventRepository)
    : PaymentEventHandlerBase<PaymentFailed>(domainEventRepository)
{
}
