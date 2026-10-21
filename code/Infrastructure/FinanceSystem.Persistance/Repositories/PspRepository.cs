using FinanceSystem.Domain.PaymentServiceProviders;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;

namespace FinanceSystem.Persistance.Repositories;

internal class PspRepository : IPspRepository
{
    private readonly DbSet<PaymentServiceProvider> _dbSet;

    public PspRepository(IDbContext context)
    {
        _dbSet = context.DbSet<PaymentServiceProvider>();
    }

    public async Task<IReadOnlyCollection<PaymentServiceProvider>> GetAllActiveAsync(CancellationToken cancellationToken)
    {
        return await _dbSet
            .Where(psp => psp.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<PaymentServiceProvider?> GetByCodeAsync(PspCode code, CancellationToken cancellationToken)
    {
        return await _dbSet.FirstOrDefaultAsync(psp => psp.Code == code, cancellationToken);
    }

    public async Task<PaymentServiceProvider?> GetHighPriorityActiveAsync(CancellationToken cancellationToken)
    {
        return await _dbSet
            .Where(psp => psp.IsActive)
            .OrderBy(psp => psp.Priority)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
