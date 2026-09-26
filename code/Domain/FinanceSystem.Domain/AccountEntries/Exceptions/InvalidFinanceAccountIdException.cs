namespace FinanceSystem.Domain.AccountEntries.Exceptions;

public class InvalidFinanceAccountIdException : BusinessException
{
    protected override int DefaultCode => AccountEntryExceptionCodes.InvalidFinanceAccountId;
}
