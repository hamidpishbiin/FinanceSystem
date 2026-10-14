namespace FinanceSystem.Domain.Payments.Exceptions;

public class InvalidPaymentDirectionException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.InvalidPaymentDirection;
}
