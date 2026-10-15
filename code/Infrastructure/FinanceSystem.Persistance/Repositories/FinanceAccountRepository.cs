using FinanceSystem.Domain.FinanceAccounts;
using FinanceSystem.Domain.FinanceAccounts.Enums;

namespace FinanceSystem.Persistance.Repositories;

public class FinanceAccountRepository(IDbContext dbContext) : IFinanceAccountRepository
{
    private DbSet<FinanceAccount> DbSet => dbContext.DbSet<FinanceAccount>();

    public async Task<FinanceAccount?> GetByUserId(long userId, CancellationToken cancellationToken)
    {
        return await DbSet.FirstOrDefaultAsync(fa => fa.UserId == userId, cancellationToken);
    }

    public async Task<FinanceAccount?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await DbSet.FirstOrDefaultAsync(fa => fa.Id == id, cancellationToken);
    }

    public async Task<FinanceAccount?> GetTopUpTargetAccountAsync(long userId, CancellationToken cancellationToken)
    {
        return await DbSet
            .FirstOrDefaultAsync(fa =>
                fa.UserId == userId &&
                fa.Type == FinanceAccountType.UserWallet &&
                fa.Status != FinanceAccountStatus.InboundFrozen,
                cancellationToken);
    }
}
