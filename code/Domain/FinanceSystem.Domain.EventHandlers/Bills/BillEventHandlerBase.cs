using FinanceSystem.Domain.Contract;
using Shared.Core.Events;

namespace FinanceSystem.Domain.EventHandlers.Bills;

public class BillEventHandlerBase<T>(IDomainEventRepository domainEventRepository)
    : EventHandlerBase<T>(domainEventRepository) where T : IDomainEvent
{
    override protected string DetermineEntityName() => "Bill";
}
