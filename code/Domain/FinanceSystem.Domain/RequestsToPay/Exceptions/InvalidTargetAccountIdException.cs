namespace FinanceSystem.Domain.RequestsToPay.Exceptions;

public class InvalidTargetAccountIdException : BusinessException
{
    protected override int DefaultCode => RequestToPayExceptionCodes.InvalidTargetAccountId;
}
