using FinanceSystem.Application.Contracts.Product.Command;
using FinanceSystem.Domain.Contract.Products;
using FinanceSystem.Domain.Products;
using FinanceSystem.Domain.Products.DomainServices;

namespace FinanceSystem.Application.Product.CommandHandlers
{
    public class ProductCreateCommandHandler : ProductCommandHandler<CreateProductCommand, ProductCreated>
    {
        private readonly StockQuantityService _stockQuantityService;

        public ProductCreateCommandHandler(
            IProductRepository repository, 
            IEventPublisher publisher, 
            IEventListener listener,
            IEventHandler<ProductCreated> eventHandler,
            StockQuantityService stockQuantityService)
            : base(repository, publisher, listener, eventHandler)
        {
            _stockQuantityService = stockQuantityService;
        }

        public override async Task Execute(CreateProductCommand command)
        {
            var productId = Guid.NewGuid();
            var model = await Domain.Products.Product.Create(
                productId,
                command.Name,
                command.Description,
                command.Price,
                command.StockQuantity, 
                Publisher,
                _stockQuantityService);

            await Repository.AddAsync(model);
        }
    }
}
