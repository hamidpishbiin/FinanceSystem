namespace FinanceSystem.Domain.Users.Exceptions;

public class UserNotFoundException : BusinessException
{
    override protected int DefaultCode => UserExceptionCodes.UserNotFound;
}
