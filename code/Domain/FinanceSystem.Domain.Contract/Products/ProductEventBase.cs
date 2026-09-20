using Shared.Core.Events;

namespace FinanceSystem.Domain.Contract.Products
{
    public abstract record ProductEventBase : DomainEvent
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
        public int StockQuantity { get; init; }
    }
}
