namespace FinanceSystem.Domain.Payments.Exceptions;

public class InvalidDestinationFinanceAccountIdException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.InvalidDestinationFinanceAccountId;
}
