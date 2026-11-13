using FinanceSystem.Domain.Contract;
using FinanceSystem.Domain.Contract.Payments;

namespace FinanceSystem.Domain.EventHandlers.Payments;

public class PaymentTokenRequestFailedEventHandler(IDomainEventRepository domainEventRepository)
    : PaymentEventHandlerBase<PaymentTokenRequestFailed>(domainEventRepository)
{
}
