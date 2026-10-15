using FinanceSystem.Domain.FinanceAccounts.Enums;

namespace FinanceSystem.Domain.FinanceAccounts;

public interface IFinanceAccountRepository : IRepository
{
    Task<FinanceAccount?> GetByUserId(long userId, CancellationToken cancellationToken);
    Task<FinanceAccount?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<FinanceAccount?> GetTopUpTargetAccountAsync(long userId, CancellationToken cancellationToken);
}
