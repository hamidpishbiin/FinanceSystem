namespace FinanceSystem.Domain.Bills;

public interface IBillRepository : IRepository
{
    Task AddAsync(Bill entity, CancellationToken cancellationToken);
    Task<Bill?> GetById(long billId, CancellationToken cancellationToken);
}
