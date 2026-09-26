namespace FinanceSystem.Domain.Users.Exceptions;

public class InvalidNationalCodeException : BusinessException
{
    protected override int DefaultCode => UserExceptionCodes.InvalidNationalCode;
}
