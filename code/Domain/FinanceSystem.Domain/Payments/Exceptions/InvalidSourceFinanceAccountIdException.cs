namespace FinanceSystem.Domain.Payments.Exceptions;

public class InvalidSourceFinanceAccountIdException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.InvalidSourceFinanceAccountId;
}
