namespace FinanceSystem.Domain.PaymentServiceProviders.Exceptions;

public class InvalidCallbackUrlException : BusinessException
{
    protected override int DefaultCode => PaymentServiceProviderExceptionCodes.InvalidCallbackUrl;
}
