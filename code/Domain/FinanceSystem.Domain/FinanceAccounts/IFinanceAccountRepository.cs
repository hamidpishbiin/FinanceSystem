namespace FinanceSystem.Domain.FinanceAccounts;

public interface IFinanceAccountRepository : IRepository
{
    Task<long?> GetCompanyWalletIdAsync(CancellationToken cancellationToken);
    Task<FinanceAccount?> GetByUserId(Guid userId, CancellationToken cancellationToken);
}
