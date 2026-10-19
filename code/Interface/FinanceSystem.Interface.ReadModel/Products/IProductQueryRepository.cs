using FinanceSystem.Domain.Products;
using Shared.Domain;

namespace FinanceSystem.Interface.ReadModel.Products
{
    public interface IProductQueryRepository : IRepository
    {
        Task<Product?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<Product>> GetAllAsync();
    }
}
