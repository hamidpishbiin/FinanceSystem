using FinanceSystem.Domain.Contract.Payments;

namespace FinanceSystem.Persistance.Repositories;

internal class PaymentEventRepository : IPaymentEventRepository
{
    private readonly DbSet<PaymentEventModel> _dbSet;

    public PaymentEventRepository(IDbContext context)
    {
        _dbSet = context.DbSet<PaymentEventModel>();
    }

    public async Task Persist(PaymentEventModel happen)
    {
        await _dbSet.AddAsync(happen);
    }
}
