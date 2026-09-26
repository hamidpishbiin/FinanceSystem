namespace FinanceSystem.Domain.FinanceAccounts.Exceptions;

public class FinanceAccountDirectionNotAllowedException : BusinessException
{
    protected override int DefaultCode => FinanceAccountExceptionCodes.FinanceAccountDirectionNotAllowed;
}
