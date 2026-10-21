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

    public async Task<FinanceAccount?> GetByUserId(long userId, CancellationToken cancellationToken)
    {
        return await _dbSet.FirstOrDefaultAsync(fa => fa.UserId == userId, cancellationToken);
    }

    public async Task<FinanceAccount?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _dbSet.FirstOrDefaultAsync(fa => fa.Id == id, cancellationToken);
    }

    public async Task<FinanceAccount?> GetTopUpTargetAccountAsync(long userId, CancellationToken cancellationToken)
    {
        return await _dbSet
            .FirstOrDefaultAsync(fa =>
                fa.UserId == userId &&
                fa.Type == FinanceAccountType.UserWallet &&
                fa.Status != FinanceAccountStatus.InboundFrozen,
                cancellationToken);
    }

    public async Task<FinanceAccount?> GetTargetAccount(long userId,
        FinanceAccountType accountType,
        List<FinanceAccountStatus> accountStatusList,
        CancellationToken cancellationToken)
    {
        return await _dbSet
            .FirstOrDefaultAsync(fa =>
                    fa.UserId == userId &&
                    fa.Type == accountType &&
                    accountStatusList.Contains(fa.Status),
                cancellationToken);
    }
}
