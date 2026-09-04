using FinanceSystem.Core;
using FinanceSystem.Interface.Contracts.Products.Models;
using Shared.Core;
using Shared.Presentation;

namespace FinanceSystem.Interface.Contracts.Products.Services
{
    public interface IProductFacadeService : IFacadeService
    {
        [HasPermission(Permissions.CreateProduct, (int)UserRoleEnum.Owner)]
        Task<JsonResponse<Guid>> Create(CreateProductModel model);


        [HasPermission(Permissions.ModifyProduct, (int)UserRoleEnum.Owner)]
        Task Modify(ModifyProductModel model);


        [HasPermission(Permissions.DeleteProduct, (int)UserRoleEnum.Owner)]
        Task Delete(DeleteProductModel model);
    }
}
