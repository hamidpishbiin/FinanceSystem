using FinanceSystem.Domain.FinanceAccounts;
using FinanceSystem.Domain.FinanceAccounts.Enums;
using FinanceSystem.Interface.ReadModel.FinanceAccounts;

namespace FinanceSystem.Persistance.Repositories;

internal class FinanceAccountQueryRepository : IFinanceAccountQueryRepository
{
    private readonly DbSet<FinanceAccount> _queryDbSet;

    public FinanceAccountQueryRepository(IQueryContext queryContext)
    {
        _queryDbSet = queryContext.DbSet<FinanceAccount>();
    }

    public async Task<IEnumerable<FinanceAccount>> GetActiveAccountsAsync(long userId, decimal billAmount, CancellationToken cancellationToken)
    {
        return await _queryDbSet
            .Where(fa => fa.UserId == userId && fa.State == FinanceAccountState.Active && fa.CachedBalance >= billAmount)
            .ToListAsync(cancellationToken);
    }
}
