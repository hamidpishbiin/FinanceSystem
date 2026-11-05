namespace FinanceSystem.Domain.PspExchangeLogs;

public interface IPspExchangeLogRepository : IRepository
{
    Task AddAsync(PspExchangeLog entity, CancellationToken cancellationToken);
}
