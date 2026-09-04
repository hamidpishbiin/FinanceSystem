using FinanceSystem.Domain.Products;

namespace FinanceSystem.Persistance.Repositories
{
    public class ProductRepository(IDbContext context) : IProductRepository
    {
        private DbSet<Product> DbSet => context.DbSet<Product>();
        

        public async Task AddAsync(Product product)
        {
            await DbSet.AddAsync(product);
        }

        public void Delete(Product product)
        {
            DbSet.Remove(product);
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await DbSet.FirstOrDefaultAsync(a => a.Id == id);
        }

        public void Update(Product product)
        {
            DbSet.Update(product);
        }

        public async Task<IReadOnlyList<Product>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }
    }
}
