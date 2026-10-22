using FinanceSystem.Domain.FinanceAccounts;
using FinanceSystem.Domain.FinanceAccounts.Enums;

namespace FinanceSystem.Persistance.Repositories;

internal class FinanceAccountRepository : IFinanceAccountRepository
{
    private readonly DbSet<FinanceAccount> _dbSet;

    public FinanceAccountRepository(IDbContext context)
    {
        _dbSet = context.DbSet<FinanceAccount>();
    }

    public async Task<List<FinanceAccount>> GetByUserIdAsync(long userId, CancellationToken cancellationToken)
    {
        return await _dbSet.Where(fa => fa.UserId == userId).ToListAsync(cancellationToken);
    }

    public async Task<FinanceAccount?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _dbSet.FirstOrDefaultAsync(fa => fa.Id == id, cancellationToken);
    }

    public async Task<FinanceAccount?> GetTargetAccount(long userId,
        FinanceAccountType accountType,
        List<FinanceAccountState> accountStateList,
        CancellationToken cancellationToken)
    {
        return await _dbSet
            .FirstOrDefaultAsync(fa =>
                    fa.UserId == userId &&
                    fa.Type == accountType &&
                    accountStateList.Contains(fa.State),
                cancellationToken);
    }
}
