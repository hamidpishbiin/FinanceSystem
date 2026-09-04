
namespace FinanceSystem.Domain.Products.Exceptions;

public class ProductNotFoundException : BusinessException
{
    public ProductNotFoundException() : base(101) { }
}