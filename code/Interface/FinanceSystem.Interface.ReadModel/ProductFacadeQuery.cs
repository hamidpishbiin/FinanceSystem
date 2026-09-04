using FinanceSystem.Domain.Products.Exceptions;
using FinanceSystem.Interface.Contracts.Products.DTOs;
using FinanceSystem.Interface.Contracts.Products.Services;
using FinanceSystem.Interface.ReadModel.Mappers;
using Shared.Core.Exceptions;
using Shared.Presentation;

namespace FinanceSystem.Interface.ReadModel
{
    public class ProductFacadeQuery(IProductQueryRepository productRepository) : IProductFacadeQuery
    {
        public async Task<JsonResponse<ProductDto>> Get(Guid id)
        {
            var product = await productRepository.GetByIdAsync(id);
            
            Guard<ProductNotFoundException>.AgainstNull(product);

            return JsonResponse<ProductDto>.Success(ProductMappers.Map(product));
        }

        public async Task<JsonResponsePagination<List<ProductDto>>> GetAll(int pageindex, int pagesize)
        {
            var products = await productRepository.GetAllAsync();
            return JsonResponsePagination<List<ProductDto>>.Success(ProductMappers.Map(products), 100, 10, 1);
        }
    }
}
