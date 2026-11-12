using Shared.Domain;

namespace FinanceSystem.Domain.Contract;

public interface IDomainEventRepository : IRepository
{
    Task AddAsync(DomainEventModel entity, CancellationToken cancellationToken);
}
