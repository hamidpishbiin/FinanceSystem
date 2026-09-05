namespace FinanceSystem.Domain.Accounts.Exceptions;

public class AccountClosedException : BusinessException
{
    protected override int DefaultCode => AccountExceptionCodes.AccountClosed;
}
