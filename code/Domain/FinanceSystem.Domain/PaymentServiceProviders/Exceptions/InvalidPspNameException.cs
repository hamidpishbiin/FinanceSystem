namespace FinanceSystem.Domain.PaymentServiceProviders.Exceptions;

public class InvalidPspNameException : BusinessException
{
    protected override int DefaultCode => PspExceptionCodes.InvalidPspName;
}
