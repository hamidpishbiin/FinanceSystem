namespace FinanceSystem.Domain.LedgerEntries.Exceptions;

public class InvalidPaymentIdException : BusinessException
{
    protected override int DefaultCode => LedgerEntryExceptionCodes.InvalidPaymentId;
}
