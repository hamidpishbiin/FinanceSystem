namespace FinanceSystem.Domain.Payments.Exceptions;

public class InvalidOriginServiceIdException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.InvalidOriginServiceId;
}
