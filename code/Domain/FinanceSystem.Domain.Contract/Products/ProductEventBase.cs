using Shared.Core.Events;

namespace FinanceSystem.Domain.Contract.Products
{
    public abstract class ProductEventBase : DomainEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
