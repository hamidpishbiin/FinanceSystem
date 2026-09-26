namespace FinanceSystem.Domain.RequestsToPay.Exceptions;

public class InvalidTargetFinanceAccountIdException : BusinessException
{
    protected override int DefaultCode => RequestToPayExceptionCodes.InvalidTargetFinanceAccountId;
}
