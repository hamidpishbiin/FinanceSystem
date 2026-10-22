namespace FinanceSystem.Domain.LedgerEntries;

public interface ILedgerEntryRepository : IRepository
{
    Task AddAsync(LedgerEntry ledgerEntry);
}
