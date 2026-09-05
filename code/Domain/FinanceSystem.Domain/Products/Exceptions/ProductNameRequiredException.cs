namespace FinanceSystem.Domain.Products.Exceptions;

public class ProductNameRequiredException : BusinessException
{
    protected override int DefaultCode => ProductExceptionCodes.ProductNameRequired;
}
