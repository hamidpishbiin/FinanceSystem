using FinanceSystem.Domain.RequestsToPay;

namespace FinanceSystem.Persistance.Repositories;

public class RequestToPayRepository(IDbContext context) : IRequestToPayRepository
{
    private DbSet<RequestToPay> DbSet => context.DbSet<RequestToPay>();

    public async Task AddAsync(RequestToPay requestToPay, CancellationToken cancellationToken)
    {
        await DbSet.AddAsync(requestToPay, cancellationToken);
    }

    public async Task<RequestToPay?> GetByReferenceNumberAsync(long referenceNumber, CancellationToken cancellationToken)
    {
        return await DbSet.FirstOrDefaultAsync(d => d.ReferenceNumber == referenceNumber, cancellationToken);
    }
}
