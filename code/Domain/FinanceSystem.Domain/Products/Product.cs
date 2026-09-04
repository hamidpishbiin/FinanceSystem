global using Shared.Core.Events;
global using Shared.Core.Exceptions;
global using Shared.Domain;

using FinanceSystem.Domain.Contract.Products;
using FinanceSystem.Domain.Products.DomainServices;
using FinanceSystem.Domain.Products.Exceptions;

namespace FinanceSystem.Domain.Products;
public sealed class Product : EntityBase<Guid>, IAggregateRoot
{
    public IEventPublisher Publisher { get; set; }

    public string Name { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public decimal Price { get; private set; } = default!;
    public int StockQuantity { get; private set; }

    protected Product() { }

    public static async Task<Product> Create(Guid id, string name, string description, decimal price, int stockQuantity, 
        IEventPublisher publisher, StockQuantityService stockQuantityService)
    {

        await stockQuantityService.CheckStockQuantity();

        var product = new Product
        {
            Id = id,
            Name = name,
            Description = description,
            Price = price,
            StockQuantity = stockQuantity,
        };

        var created = new ProductCreated()
        {
            Id = id,
            Description = description,
            Price = price,
            Name = name,
            StockQuantity = stockQuantity,
        };
        await publisher.Publish(created);
        
        return product;
    }

    public async Task Update(string name, string description, decimal price, int stockQuantity, IEventPublisher publisher)
    {
        SetProperties(name, description, price, stockQuantity);

        var modified = new ProductModified() 
        {
            Id = Id,
            Description = description,
            Price = price,
            Name = name,
            StockQuantity = stockQuantity,
        };
        await publisher.Publish(modified);
    }

    private void SetProperties(string name, string description, decimal price, int stockQuantity)
    {
        Guard<ProductNameRequiredException>.AgainstNullOrEmpty(name);
        Name = name;
        Description = description;
        Price = price;
        StockQuantity = stockQuantity;
    }
}
