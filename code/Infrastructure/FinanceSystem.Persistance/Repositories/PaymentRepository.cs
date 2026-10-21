using FinanceSystem.Domain.Payments;

namespace FinanceSystem.Persistance.Repositories;

internal class PaymentRepository : IPaymentRepository
{
    private readonly DbSet<Payment> _dbSet;

    public PaymentRepository(IDbContext context)
    {
        _dbSet = context.DbSet<Payment>();
    }

    public async Task AddAsync(Payment payment)
    {
        await _dbSet.AddAsync(payment);
    }

    public async Task<Payment?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _dbSet.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<Payment?> GetByReferenceNumberAsync(string referenceNumber, CancellationToken cancellationToken)
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.ReferenceNumber == referenceNumber, cancellationToken);
    }
}
