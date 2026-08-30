using ProductManagement.Domain.Contract.Payments;

namespace ProductManagement.Persistance.Repositories;

public class PaymentEventRepository(IDbContext context) : IPaymentEventRepository
{
    public async Task Persist(PaymentEventModel happen)
    {
        await context.DbSet<PaymentEventModel>().AddAsync(happen);
    }
}
