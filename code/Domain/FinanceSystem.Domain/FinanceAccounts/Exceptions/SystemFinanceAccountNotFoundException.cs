namespace FinanceSystem.Domain.FinanceAccounts.Exceptions;

public class SystemFinanceAccountNotFoundException : BusinessException
{
    protected override int DefaultCode => FinanceAccountExceptionCodes.SystemFinanceAccountNotFound;
}
