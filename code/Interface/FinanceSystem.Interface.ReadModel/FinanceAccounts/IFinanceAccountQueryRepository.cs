using FinanceSystem.Domain.FinanceAccounts;
using Shared.Domain;

namespace FinanceSystem.Interface.ReadModel.FinanceAccounts;

public interface IFinanceAccountQueryRepository : IRepository
{
    Task<IEnumerable<FinanceAccount>> GetActiveAccountsAsync(long userId, decimal billAmount, CancellationToken cancellationToken);
}
