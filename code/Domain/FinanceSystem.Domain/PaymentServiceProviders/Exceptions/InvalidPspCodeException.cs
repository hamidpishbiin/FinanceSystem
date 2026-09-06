namespace FinanceSystem.Domain.PaymentServiceProviders.Exceptions;

public class InvalidPspCodeException : BusinessException
{
    protected override int DefaultCode => PaymentServiceProviderExceptionCodes.InvalidPspCode;
}
