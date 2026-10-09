using FinanceSystem.Domain.Payments;

namespace FinanceSystem.Persistance.Repositories;

public class PaymentRepository(IDbContext context) : IPaymentRepository
{
    private DbSet<Payment> DbSet => context.DbSet<Payment>();

    public async Task AddAsync(Payment payment)
    {
        await DbSet.AddAsync(payment);
    }

    public async Task<Payment?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await DbSet.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<Payment?> GetByReferenceNumberAsync(string referenceNumber, CancellationToken cancellationToken)
    {
        return await DbSet.FirstOrDefaultAsync(p => p.ReferenceNumber == referenceNumber, cancellationToken);
    }
}
