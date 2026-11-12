using FinanceSystem.Domain.Contract;
using FinanceSystem.Domain.Contract.Payments;

namespace FinanceSystem.Domain.EventHandlers.PaymentEventHandlers;

public class PaymentCreatedEventHandler(IDomainEventRepository domainEventRepository)
    : EventHandlerBase<PaymentCreated>(EventEntityNames.Payment, domainEventRepository)
{
}
