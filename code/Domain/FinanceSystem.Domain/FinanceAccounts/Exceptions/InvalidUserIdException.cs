namespace FinanceSystem.Domain.FinanceAccounts.Exceptions;

public class InvalidUserIdException : BusinessException
{
    protected override int DefaultCode => FinanceAccountExceptionCodes.InvalidUserId;
}
