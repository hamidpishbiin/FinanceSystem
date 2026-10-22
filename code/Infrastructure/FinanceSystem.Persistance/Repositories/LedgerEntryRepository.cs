using FinanceSystem.Domain.LedgerEntries;

namespace FinanceSystem.Persistance.Repositories;

internal class LedgerEntryRepository : ILedgerEntryRepository
{
    private readonly DbSet<LedgerEntry> _dbSet;

    public LedgerEntryRepository(IDbContext context)
    {
        _dbSet = context.DbSet<LedgerEntry>();
    }

    public async Task AddAsync(LedgerEntry ledgerEntry)
    {
        await _dbSet.AddAsync(ledgerEntry);
    }
}
