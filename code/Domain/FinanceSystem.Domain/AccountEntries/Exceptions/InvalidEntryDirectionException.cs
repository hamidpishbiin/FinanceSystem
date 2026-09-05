namespace FinanceSystem.Domain.AccountEntries.Exceptions;

public class InvalidEntryDirectionException : BusinessException
{
    protected override int DefaultCode => AccountEntryExceptionCodes.InvalidEntryDirection;
}
