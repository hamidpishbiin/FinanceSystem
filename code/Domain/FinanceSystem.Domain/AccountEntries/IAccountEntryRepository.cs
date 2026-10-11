namespace FinanceSystem.Domain.AccountEntries;

public interface IAccountEntryRepository : IRepository
{
    Task AddAsync(AccountEntry accountEntry);
}
