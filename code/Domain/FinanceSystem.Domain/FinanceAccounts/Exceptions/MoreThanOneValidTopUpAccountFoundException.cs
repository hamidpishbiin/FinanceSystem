namespace FinanceSystem.Domain.FinanceAccounts.Exceptions;

public class MoreThanOneValidTopUpAccountFoundException : BusinessException
{
    override protected int DefaultCode => FinanceAccountExceptionCodes.MoreThanOneValidTopUpAccountFound;
}
