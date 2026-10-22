namespace FinanceSystem.Domain.LedgerEntries.Exceptions;

public class InvalidEntryDirectionException : BusinessException
{
    protected override int DefaultCode => LedgerEntryExceptionCodes.InvalidEntryDirection;
}
