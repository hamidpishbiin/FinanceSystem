namespace FinanceSystem.Domain.Payments.Exceptions;

public class InvalidServiceIdException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.InvalidServiceId;
}
