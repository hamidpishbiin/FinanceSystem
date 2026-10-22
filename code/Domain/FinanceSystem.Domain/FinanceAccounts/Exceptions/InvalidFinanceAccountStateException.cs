namespace FinanceSystem.Domain.FinanceAccounts.Exceptions;

public class InvalidFinanceAccountStateException : BusinessException
{
    override protected int DefaultCode => FinanceAccountExceptionCodes.InvalidFinanceAccountState;
}
