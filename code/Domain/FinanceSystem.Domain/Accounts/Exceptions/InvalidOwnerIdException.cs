namespace FinanceSystem.Domain.Accounts.Exceptions;

public class InvalidOwnerIdException : BusinessException
{
    protected override int DefaultCode => AccountExceptionCodes.InvalidOwnerId;
}
