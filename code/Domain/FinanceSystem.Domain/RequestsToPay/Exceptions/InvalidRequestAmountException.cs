namespace FinanceSystem.Domain.RequestsToPay.Exceptions;

public class InvalidRequestAmountException : BusinessException
{
    protected override int DefaultCode => RequestToPayExceptionCodes.InvalidRequestAmount;
}
