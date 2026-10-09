namespace FinanceSystem.Domain.Payments.Exceptions;

public class InvalidExternalServiceIdException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.InvalidExternalServiceId;
}
