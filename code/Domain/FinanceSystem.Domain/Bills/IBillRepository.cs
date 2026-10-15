namespace FinanceSystem.Domain.Bills;

public interface IBillRepository : IRepository
{
    Task<Bill?> GetById(long billId, CancellationToken cancellationToken);
}
