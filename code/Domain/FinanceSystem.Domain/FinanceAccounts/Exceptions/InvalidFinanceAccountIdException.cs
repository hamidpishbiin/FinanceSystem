namespace FinanceSystem.Domain.FinanceAccounts.Exceptions;

public class InvalidFinanceAccountIdException : BusinessException
{
    protected override int DefaultCode => FinanceAccountExceptionCodes.InvalidFinanceAccountId;
}
