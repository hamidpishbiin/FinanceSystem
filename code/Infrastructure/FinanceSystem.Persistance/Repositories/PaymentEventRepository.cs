using FinanceSystem.Domain.Contract.Payments;

namespace FinanceSystem.Persistance.Repositories;

public class PaymentEventRepository(IDbContext context) : IPaymentEventRepository
{
    public async Task Persist(PaymentEventModel happen)
    {
        await context.DbSet<PaymentEventModel>().AddAsync(happen);
    }
}
