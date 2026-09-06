namespace FinanceSystem.Domain.Payments.Exceptions;

public class InvalidPaymentPurposeException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.InvalidPaymentPurpose;
}
