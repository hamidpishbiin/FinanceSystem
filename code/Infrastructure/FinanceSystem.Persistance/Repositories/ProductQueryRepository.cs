using FinanceSystem.Domain.Products;
using FinanceSystem.Interface.ReadModel;

namespace FinanceSystem.Persistance.Repositories
{
    public class ProductQueryRepository(IQueryContext queryContext) : IProductQueryRepository
    {
        private DbSet<Product> QueryDbSet => queryContext.DbSet<Product>();

        public async Task<IReadOnlyList<Product>> GetAllAsync()
        {
            return await QueryDbSet.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await QueryDbSet.FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
