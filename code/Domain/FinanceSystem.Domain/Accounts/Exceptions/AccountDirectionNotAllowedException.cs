namespace FinanceSystem.Domain.Accounts.Exceptions;

public class AccountDirectionNotAllowedException : BusinessException
{
    protected override int DefaultCode => AccountExceptionCodes.AccountDirectionNotAllowed;
}
