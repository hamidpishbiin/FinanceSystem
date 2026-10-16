using FinanceSystem.Domain.Bills;

namespace FinanceSystem.Persistance.Repositories;

public class BillRepository(IDbContext context) : IBillRepository
{
    private DbSet<Bill> DbSet => context.DbSet<Bill>();

    public async Task AddAsync(Bill entity, CancellationToken cancellationToken)
    {
        await DbSet.AddAsync(entity, cancellationToken);
    }

    public Task<Bill?> GetById(long billId, CancellationToken cancellationToken)
    {
        return DbSet.FirstOrDefaultAsync(b => b.Id == billId, cancellationToken);
    }
}
