using FinanceSystem.Domain.FinanceAccounts;
using FinanceSystem.Domain.FinanceAccounts.Enums;

namespace FinanceSystem.Persistance.Repositories;

public class FinanceAccountRepository(IDbContext dbContext) : IFinanceAccountRepository
{
    private DbSet<FinanceAccount> DbSet => dbContext.DbSet<FinanceAccount>();

    public async Task<long?> GetCompanyWalletIdAsync(CancellationToken cancellationToken)
    {
        return await DbSet
            .Where(account => account.Type == FinanceAccountType.CompanyWallet)
            .Select(account => (long?)account.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
