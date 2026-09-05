namespace FinanceSystem.Domain.Accounts.Exceptions;

public class InsufficientBalanceException : BusinessException
{
    protected override int DefaultCode => AccountExceptionCodes.InsufficientBalance;
}
