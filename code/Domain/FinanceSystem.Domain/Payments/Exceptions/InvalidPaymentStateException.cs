namespace FinanceSystem.Domain.Payments.Exceptions;

public class InvalidPaymentStateException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.InvalidPaymentState;
}
