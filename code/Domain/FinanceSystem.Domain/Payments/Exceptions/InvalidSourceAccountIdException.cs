namespace FinanceSystem.Domain.Payments.Exceptions;

public class InvalidSourceAccountIdException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.InvalidSourceAccountId;
}
