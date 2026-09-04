using FinanceSystem.Core;
using FinanceSystem.Interface.Contracts.Products.DTOs;
using Shared.Core;
using Shared.Presentation;

namespace FinanceSystem.Interface.Contracts.Products.Services
{
    public interface IProductFacadeQuery : IFacadeService
    {
        [HasPermission(Permissions.AccessProduct, (int)UserRoleEnum.Owner)]
        Task<JsonResponse<ProductDto>> Get(Guid id);


        // [HasPermission(Permissions.AccessProduct, (int)UserRoleEnum.Owner)]
        [IgnorePermission]
        Task<JsonResponsePagination<List<ProductDto>>> GetAll(int pageIndex, int pageSize);
    }
}
