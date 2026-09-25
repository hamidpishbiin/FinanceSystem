namespace FinanceSystem.Domain.RequestsToPay;

public interface IRequestToPayRepository : IRepository
{
    Task AddAsync(RequestToPay requestToPay, CancellationToken cancellationToken);
    Task<RequestToPay?> GetByReferenceNumberAsync(long referenceNumber, CancellationToken cancellationToken);
}
