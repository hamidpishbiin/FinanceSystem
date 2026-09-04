using FinanceSystem.Application.Contracts.Product.Command;
using FinanceSystem.Domain.Contract.Products;
using FinanceSystem.Domain.Products;
using FinanceSystem.Domain.Products.Exceptions;

namespace FinanceSystem.Application.Product.CommandHandlers
{
    public class ProductModifyCommandHandler : ProductCommandHandler<ModifyProductCommand, ProductModified>
    {
        public ProductModifyCommandHandler(
            IProductRepository repository, 
            IEventPublisher publisher, 
            IEventListener listener,
            IEventHandler<ProductModified> eventHandler)
            : base(repository, publisher, listener, eventHandler)
        {
        }

        public override async Task Execute(ModifyProductCommand command)
        {
            var product = await Repository.GetByIdAsync(command.Id);

            Guard<ProductNotFoundException>.AgainstNull(product);

            await product.Update(command.Name, command.Description, command.Price, command.StockQuantity, Publisher);
        }
    }
}
