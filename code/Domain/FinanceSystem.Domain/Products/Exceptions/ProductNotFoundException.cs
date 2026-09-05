namespace FinanceSystem.Domain.Products.Exceptions;

public class ProductNotFoundException : BusinessException
{
    protected override int DefaultCode => ProductExceptionCodes.ProductNotFound;
}
