using FinanceSystem.Domain.FinanceAccounts;
using Shared.Domain;

namespace FinanceSystem.Interface.ReadModel.FinanceAccounts;

public interface IFinanceAccountQueryRepository : IRepository
{
    Task<List<FinanceAccount>> GetUserAccountsAsync(long userId, CancellationToken cancellationToken);
}
