using FinanceSystem.Domain.Contract;
using FinanceSystem.Domain.Contract.Payments;

namespace FinanceSystem.Domain.EventHandlers.Payments;

public class PaymentCreatedEventHandler(IDomainEventRepository domainEventRepository)
    : PaymentEventHandlerBase<PaymentCreated>(domainEventRepository)
{
}
