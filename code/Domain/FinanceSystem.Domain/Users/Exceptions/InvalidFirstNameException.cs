namespace FinanceSystem.Domain.Users.Exceptions;

public class InvalidFirstNameException : BusinessException
{
    protected override int DefaultCode => UserExceptionCodes.InvalidFirstName;
}
