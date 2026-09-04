using FinanceSystem.Domain.Products;
using Shared.Core.Events;
using NSubstitute;
using FinanceSystem.Domain.Products.DomainServices;

namespace FinanceSystem.Domain.Tests.Unit.ProductsUnitTests
{
    public class ProductBuilder
    {
        private Guid Id { get; set; } = Guid.Parse("6B29FC40-CA47-1067-B31D-00DD010662DA");
        private string Name { get; set; } = default!;
        private string Description { get; set; } = default!;
        private decimal Price { get; set; } = default!;
        private int StockQuantity { get; set; }

        private IEventPublisher _eventPublisher => Substitute.For<IEventPublisher>();
        private StockQuantityService _stockQuantityService => Substitute.For<StockQuantityService>();

        public async Task<Product> Build()
        {
            return await Product.Create(Id, Name,Description,Price,StockQuantity, _eventPublisher,_stockQuantityService);
        }

        public ProductBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public ProductBuilder WithDescription(string description)
        {
            Description = description;
            return this;
        }
        public ProductBuilder WithPrice(decimal price)
        {
            Price = price;
            return this;
        }
        public ProductBuilder WithStockQuantity(int quantity)
        {
            StockQuantity = quantity;
            return this;
        }
    }
}