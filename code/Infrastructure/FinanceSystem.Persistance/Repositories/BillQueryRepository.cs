using FinanceSystem.Domain.Bills;
using FinanceSystem.Interface.ReadModel.Bills;

namespace FinanceSystem.Persistance.Repositories;

internal class BillQueryRepository : IBillQueryRepository
{
    private readonly DbSet<Bill> _queryDbSet;

    public BillQueryRepository(IQueryContext queryContext)
    {
        _queryDbSet = queryContext.DbSet<Bill>();
    }

    public async Task<Bill?> GetByIdAsync(long billId, CancellationToken cancellationToken)
    {
        return await _queryDbSet.FirstOrDefaultAsync(b => b.Id == billId, cancellationToken);
    }

    public async Task<Bill?> GetAsync(long billId, long userId, CancellationToken cancellationToken)
    {
        return await _queryDbSet.FirstOrDefaultAsync(b => b.Id == billId && b.UserId == userId, cancellationToken);
    }
}
