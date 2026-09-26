namespace FinanceSystem.Domain.FinanceAccounts.Exceptions;

public class FinanceAccountClosedException : BusinessException
{
    protected override int DefaultCode => FinanceAccountExceptionCodes.FinanceAccountClosed;
}
