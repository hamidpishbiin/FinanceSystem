namespace FinanceSystem.Domain.Users.Exceptions;

public class InvalidLastNameException : BusinessException
{
    protected override int DefaultCode => UserExceptionCodes.InvalidLastName;
}
