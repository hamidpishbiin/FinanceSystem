using FinanceSystem.Interface.Contracts.Products.DTOs;
using FinanceSystem.Interface.Contracts.Products.Services;

namespace FinanceSystem.Presentation.Controllers
{
    [Route("api/product")]
    [ApiController]
    // [Authorize]
    public class ProductQueryController(IProductFacadeQuery facadeService) : ControllerBase
    {
        [HttpGet("getby/{id}")]
        public async Task<JsonResponse<ProductDto>> Get([FromRoute] Guid id)
        {
            return await facadeService.Get(id);
        }
        
        [HttpGet("getall")]
        public async Task<JsonResponsePagination<List<ProductDto>>> GetAll(int pageIndex, int pageSize)
        {
            return await facadeService.GetAll(pageIndex,pageSize);
        }
    }
}
