using FinanceSystem.Domain.FinanceAccounts.Enums;

namespace FinanceSystem.Domain.FinanceAccounts;

public interface IFinanceAccountRepository : IRepository
{
    Task<FinanceAccount?> GetByUserId(Guid userId, CancellationToken cancellationToken);
    Task<FinanceAccount?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<FinanceAccount?> GetTopUpTargetAccountAsync(Guid userId, CancellationToken cancellationToken);
}
