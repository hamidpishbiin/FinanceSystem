namespace FinanceSystem.Domain.Users.Exceptions;

public class InvalidPhoneNumberException : BusinessException
{
    protected override int DefaultCode => UserExceptionCodes.InvalidPhoneNumber;
}
