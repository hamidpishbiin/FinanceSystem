using FinanceSystem.Domain.Contract;
using FinanceSystem.Domain.Contract.Payments;

namespace FinanceSystem.Domain.EventHandlers.Payments;

public class PaymentSucceededEventHandler(IDomainEventRepository domainEventRepository)
    : PaymentEventHandlerBase<PaymentSucceeded>(domainEventRepository)
{
}
