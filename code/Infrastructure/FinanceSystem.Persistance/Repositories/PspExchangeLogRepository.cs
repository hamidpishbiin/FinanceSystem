using FinanceSystem.Domain.PspExchangeLogs;

namespace FinanceSystem.Persistance.Repositories;

public class PspExchangeLogRepository : IPspExchangeLogRepository
{
    private readonly DbSet<PspExchangeLog> _dbSet;

    public PspExchangeLogRepository(IDbContext context)
    {
        _dbSet = context.DbSet<PspExchangeLog>();
    }

    public async Task AddAsync(PspExchangeLog entity, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }
}
