namespace FinanceSystem.Domain.AccountEntries.Exceptions;

public class InvalidPaymentIdException : BusinessException
{
    protected override int DefaultCode => AccountEntryExceptionCodes.InvalidPaymentId;
}
