namespace FinanceSystem.Domain.PaymentServiceProviders.Exceptions;

public class InvalidPspTokenException : BusinessException
{
    override protected int DefaultCode => PspExceptionCodes.InvalidToken;
}
