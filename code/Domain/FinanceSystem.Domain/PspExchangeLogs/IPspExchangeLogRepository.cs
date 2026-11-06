namespace FinanceSystem.Domain.PspExchangeLogs;

public interface IPspExchangeLogRepository : IRepository
{
    void Add(PspExchangeLog entity);
    Task AddAsync(PspExchangeLog entity, CancellationToken cancellationToken);
}
