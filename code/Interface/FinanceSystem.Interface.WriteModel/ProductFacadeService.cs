global using Shared.Application;
global using Shared.Core.EventHandlers;
global using Shared.Core.Events;
global using Shared.Presentation;

using FinanceSystem.Application.Contracts.Product.Command;
using FinanceSystem.Domain.Contract.Products;
using FinanceSystem.Interface.Contracts.Products.Models;
using FinanceSystem.Interface.Contracts.Products.Services;

namespace FinanceSystem.Interface.WriteModel
{
    public class ProductFacadeService(
        ICommandBus _commandBus,
        IEventListener _listener) : IProductFacadeService
    {
        public async Task<JsonResponse<Guid>> Create(CreateProductModel model)
        {
            Guid productId = default;
            await _listener.Subscribe(new ActionEventHandler<ProductCreated>(a =>
            {
                productId = a.Id;
            }));

            await _commandBus.Dispatch(new CreateProductCommand
            {
                Description = model.Description,
                Name = model.Name,
                Price = model.Price,
                StockQuantity = model.StockQuantity
            });
            return JsonResponse<Guid>.Success(productId);
        }

        public async Task Modify(ModifyProductModel model)
        {
            await _commandBus.Dispatch(new ModifyProductCommand
            {
                Id = model.Id,
                Description = model.Description,
                Name = model.Name,
                Price = model.Price,
                StockQuantity = model.StockQuantity
            });
        }

        public Task Delete(DeleteProductModel model)
        {
            throw new NotImplementedException();
        }
    }
}
