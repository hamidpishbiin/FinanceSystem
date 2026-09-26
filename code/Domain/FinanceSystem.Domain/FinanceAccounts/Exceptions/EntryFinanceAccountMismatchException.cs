namespace FinanceSystem.Domain.FinanceAccounts.Exceptions;

public class EntryFinanceAccountMismatchException : BusinessException
{
    protected override int DefaultCode => FinanceAccountExceptionCodes.EntryFinanceAccountMismatch;
}
