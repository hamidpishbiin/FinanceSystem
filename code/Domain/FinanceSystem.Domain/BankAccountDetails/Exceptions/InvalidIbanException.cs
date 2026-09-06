namespace FinanceSystem.Domain.BankAccountDetails.Exceptions;

public class InvalidIbanException : BusinessException
{
    protected override int DefaultCode => BankAccountDetailExceptionCodes.InvalidIban;
}
