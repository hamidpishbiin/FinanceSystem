namespace FinanceSystem.Domain.FinanceAccounts.Exceptions;

public class ValidTopUpAccountNotFoundException : BusinessException
{
    override protected int DefaultCode => FinanceAccountExceptionCodes.ValidTopUpAccountNotFound;
}
