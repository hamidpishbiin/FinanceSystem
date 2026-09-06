namespace FinanceSystem.Domain.Payments.Exceptions;

public class MissingBankPaymentDetailException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.MissingBankPaymentDetail;
}
