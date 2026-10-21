using FinanceSystem.Domain.Bills;

namespace FinanceSystem.Persistance.Repositories;

internal class BillRepository : IBillRepository
{
    private readonly DbSet<Bill> _dbSet;

    public BillRepository(IDbContext context)
    {
        _dbSet = context.DbSet<Bill>();
    }

    public async Task AddAsync(Bill entity, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public Task<Bill?> GetById(long billId, CancellationToken cancellationToken)
    {
        return _dbSet.FirstOrDefaultAsync(b => b.Id == billId, cancellationToken);
    }
}
