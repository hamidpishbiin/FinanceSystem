using FinanceSystem.Domain.PaymentServiceProviders;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;

namespace FinanceSystem.Persistance.Repositories;

public class PspRepository(IDbContext dbContext) : IPspRepository
{
    private DbSet<PaymentServiceProvider> DbSet => dbContext.DbSet<PaymentServiceProvider>();

    public async Task<IReadOnlyCollection<PaymentServiceProvider>> GetAllActiveAsync(CancellationToken cancellationToken)
    {
        return await DbSet
            .Where(psp => psp.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<PaymentServiceProvider?> GetByCodeAsync(PspCode code, CancellationToken cancellationToken)
    {
        return await DbSet.FirstOrDefaultAsync(psp => psp.Code == code, cancellationToken);
    }

    public async Task<PaymentServiceProvider?> GetHighPriorityActiveAsync(CancellationToken cancellationToken)
    {
        return await DbSet
            .Where(psp => psp.IsActive)
            .OrderBy(psp => psp.Priority)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
