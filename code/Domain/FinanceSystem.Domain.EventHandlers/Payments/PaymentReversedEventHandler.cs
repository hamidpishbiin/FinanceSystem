using FinanceSystem.Domain.Contract;
using FinanceSystem.Domain.Contract.Payments;

namespace FinanceSystem.Domain.EventHandlers.Payments;

public class PaymentReversedEventHandler(IDomainEventRepository domainEventRepository)
    : PaymentEventHandlerBase<PaymentReversed>(domainEventRepository)
{
}
