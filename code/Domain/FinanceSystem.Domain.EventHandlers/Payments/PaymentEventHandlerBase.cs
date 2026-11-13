using FinanceSystem.Domain.Contract;
using Shared.Core.Events;

namespace FinanceSystem.Domain.EventHandlers.Payments;

public class PaymentEventHandlerBase<T>(IDomainEventRepository domainEventRepository)
    : EventHandlerBase<T>(domainEventRepository) where T : IDomainEvent
{
    override protected string DetermineEntityName() => "Payment";
}
