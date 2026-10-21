using FinanceSystem.Domain.AccountEntries;

namespace FinanceSystem.Persistance.Repositories;

internal class AccountEntryRepository : IAccountEntryRepository
{
    private readonly DbSet<AccountEntry> _dbSet;

    public AccountEntryRepository(IDbContext context)
    {
        _dbSet = context.DbSet<AccountEntry>();
    }

    public async Task AddAsync(AccountEntry accountEntry)
    {
        await _dbSet.AddAsync(accountEntry);
    }
}
