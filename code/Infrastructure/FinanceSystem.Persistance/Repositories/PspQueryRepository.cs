using FinanceSystem.Domain.PaymentServiceProviders;
using FinanceSystem.Interface.ReadModel.PaymentServiceProviders;

namespace FinanceSystem.Persistance.Repositories;

internal class PspQueryRepository : IPspQueryRepository
{
    private readonly DbSet<PaymentServiceProvider> _queryDbSet;

    public PspQueryRepository(IQueryContext queryContext)
    {
        _queryDbSet = queryContext.DbSet<PaymentServiceProvider>();
    }

    public async Task<List<PaymentServiceProvider>> GetActivePspsAsync(CancellationToken cancellationToken)
    {
        return await _queryDbSet.Where(psp => psp.IsActive).ToListAsync(cancellationToken);
    }
}
