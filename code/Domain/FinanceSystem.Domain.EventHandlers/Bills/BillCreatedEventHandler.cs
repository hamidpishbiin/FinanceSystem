using FinanceSystem.Domain.Contract;
using FinanceSystem.Domain.Contract.Bills;

namespace FinanceSystem.Domain.EventHandlers.Bills;

public class BillCreatedEventHandler(IDomainEventRepository domainEventRepository)
    : BillEventHandlerBase<BillCreated>(domainEventRepository)
{

}
