using FinanceSystem.Domain;
using FinanceSystem.Domain.Contract;

namespace FinanceSystem.Persistance.Repositories;

public class DomainEventRepository(IDbContext context) : IDomainEventRepository
{
    private readonly DbSet<DomainEventModel> _dbSet = context.DbSet<DomainEventModel>();

    public async Task AddAsync(DomainEventModel entity, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }
}
