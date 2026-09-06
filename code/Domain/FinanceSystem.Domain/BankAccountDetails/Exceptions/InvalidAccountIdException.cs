namespace FinanceSystem.Domain.BankAccountDetails.Exceptions;

public class InvalidAccountIdException : BusinessException
{
    protected override int DefaultCode => BankAccountDetailExceptionCodes.InvalidAccountId;
}
