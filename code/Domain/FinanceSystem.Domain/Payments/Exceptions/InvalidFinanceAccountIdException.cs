namespace FinanceSystem.Domain.Payments.Exceptions;

public class InvalidFinanceAccountIdException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.InvalidFinanceAccountId;
}
