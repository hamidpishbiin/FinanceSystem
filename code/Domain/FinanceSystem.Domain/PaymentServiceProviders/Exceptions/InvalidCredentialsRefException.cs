namespace FinanceSystem.Domain.PaymentServiceProviders.Exceptions;

public class InvalidCredentialsRefException : BusinessException
{
    protected override int DefaultCode => PaymentServiceProviderExceptionCodes.InvalidCredentialsRef;
}
