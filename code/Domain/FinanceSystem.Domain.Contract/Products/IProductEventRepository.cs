using Shared.Domain;

namespace FinanceSystem.Domain.Contract.Products
{
    public interface IProductEventRepository : IRepository
    {
        Task Persist(ProductEventModel happen);
    }
}
