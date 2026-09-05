namespace FinanceSystem.Domain.AccountEntries.Exceptions;

public class InvalidAccountIdException : BusinessException
{
    protected override int DefaultCode => AccountEntryExceptionCodes.InvalidAccountId;
}
