namespace FinanceSystem.Domain.Payments.Exceptions;

public class InvalidPaymentAmountException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.InvalidPaymentAmount;
}
