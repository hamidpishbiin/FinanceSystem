namespace FinanceSystem.Domain.AccountEntries.Exceptions;

public class InvalidAmountException : BusinessException
{
    protected override int DefaultCode => AccountEntryExceptionCodes.InvalidAmount;
}
