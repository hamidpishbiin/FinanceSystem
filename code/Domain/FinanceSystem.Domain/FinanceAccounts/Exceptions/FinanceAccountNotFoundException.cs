namespace FinanceSystem.Domain.FinanceAccounts.Exceptions;

public class FinanceAccountNotFoundException : BusinessException
{
    override protected int DefaultCode => FinanceAccountExceptionCodes.FinanceAccountNotFound;
}
