
namespace FinanceSystem.Domain.Products.Exceptions;

public class ProductNameRequiredException : BusinessException
{
    public ProductNameRequiredException() : base(100) { }
}
