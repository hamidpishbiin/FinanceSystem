using FinanceSystem.Domain.Contract.Products;

namespace FinanceSystem.Persistance.Repositories
{
    public class ProductEventRepository(IDbContext context) : IProductEventRepository
    {
        public async Task Persist(ProductEventModel happen)
        {
            await context.DbSet<ProductEventModel>().AddAsync(happen);
        }
    }
}
