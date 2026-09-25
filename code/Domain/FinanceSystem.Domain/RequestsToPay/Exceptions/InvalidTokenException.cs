namespace FinanceSystem.Domain.RequestsToPay.Exceptions;

public class InvalidTokenException : BusinessException
{
    protected override int DefaultCode => RequestToPayExceptionCodes.InvalidToken;
}
