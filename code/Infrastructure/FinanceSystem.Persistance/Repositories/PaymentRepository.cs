using FinanceSystem.Domain.Payments;

namespace FinanceSystem.Persistance.Repositories;

public class PaymentRepository(IDbContext context) : IPaymentRepository
{
    private DbSet<Payment> DbSet => context.DbSet<Payment>();

    public async Task AddAsync(Payment payment)
    {
        await DbSet.AddAsync(payment);
    }

    public void Delete(Payment payment)
    {
        DbSet.Remove(payment);
    }

    public async Task<Payment?> GetByIdAsync(long id)
    {
        return await DbSet.FirstOrDefaultAsync(a => a.Id == id);
    }

    public void Update(Payment payment)
    {
        DbSet.Update(payment);
    }
}
