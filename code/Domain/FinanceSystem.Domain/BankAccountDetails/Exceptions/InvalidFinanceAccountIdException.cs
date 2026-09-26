namespace FinanceSystem.Domain.BankAccountDetails.Exceptions;

public class InvalidFinanceAccountIdException : BusinessException
{
    protected override int DefaultCode => BankAccountDetailExceptionCodes.InvalidFinanceAccountId;
}
