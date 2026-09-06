namespace FinanceSystem.Domain.Payments.Exceptions;

public class InvalidDestinationAccountIdException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.InvalidDestinationAccountId;
}
