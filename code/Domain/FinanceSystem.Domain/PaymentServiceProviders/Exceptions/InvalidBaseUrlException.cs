namespace FinanceSystem.Domain.PaymentServiceProviders.Exceptions;

public class InvalidBaseUrlException : BusinessException
{
    protected override int DefaultCode => PaymentServiceProviderExceptionCodes.InvalidBaseUrl;
}
