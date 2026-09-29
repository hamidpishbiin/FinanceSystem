namespace FinanceSystem.Domain.FinanceAccounts.Exceptions;

public class SystemFinanceAccountOwnerMismatchException : BusinessException
{
    protected override int DefaultCode => FinanceAccountExceptionCodes.SystemFinanceAccountOwnerMismatch;
}
