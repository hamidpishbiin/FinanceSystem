namespace FinanceSystem.Domain.Payments.Exceptions;

public class InvalidTokenException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.InvalidToken;
}
