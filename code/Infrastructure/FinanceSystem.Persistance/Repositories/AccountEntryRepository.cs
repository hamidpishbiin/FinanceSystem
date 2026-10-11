using FinanceSystem.Domain.AccountEntries;

namespace FinanceSystem.Persistance.Repositories;

public class AccountEntryRepository(IDbContext context) : IAccountEntryRepository
{
    private DbSet<AccountEntry> DbSet => context.DbSet<AccountEntry>();

    public async Task AddAsync(AccountEntry accountEntry)
    {
        await DbSet.AddAsync(accountEntry);
    }
}
