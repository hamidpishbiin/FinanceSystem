using FinanceSystem.Domain.Bills;
using Shared.Domain;

namespace FinanceSystem.Interface.ReadModel.Bills;

public interface IBillQueryRepository : IRepository
{
    Task<Bill?> GetByIdAsync(long billId, CancellationToken cancellationToken);
    Task<Bill?> GetAsync(long billId, long userId, CancellationToken cancellationToken);
}
