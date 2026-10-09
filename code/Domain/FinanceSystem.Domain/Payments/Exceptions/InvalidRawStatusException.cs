namespace FinanceSystem.Domain.Payments.Exceptions;

public class InvalidRawStatusException : BusinessException
{
    override protected int DefaultCode => PaymentExceptionCodes.InvalidRawStatus;
}
