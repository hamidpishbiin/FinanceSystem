namespace FinanceSystem.Domain.Accounts.Exceptions;

public class EntryAccountMismatchException : BusinessException
{
    protected override int DefaultCode => AccountExceptionCodes.EntryAccountMismatch;
}
