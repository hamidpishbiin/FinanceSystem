namespace FinanceSystem.Domain.Payments.Exceptions;

public class InvalidPaymentPurchaseException : BusinessException
{
    override protected int DefaultCode => PaymentExceptionCodes.InvalidPaymentPurchase;
}
