using FinanceSystem.Domain.FinanceAccounts.Enums;

namespace FinanceSystem.Domain.FinanceAccounts;

public interface IFinanceAccountRepository : IRepository
{
    Task<List<FinanceAccount>> GetByUserIdAsync(long userId, CancellationToken cancellationToken);
    Task<FinanceAccount?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<FinanceAccount?> GetTargetAccount(long userId, FinanceAccountType accountType, List<FinanceAccountState> accountStateList, CancellationToken cancellationToken);
}
