namespace FinanceSystem.Domain.Payments.Exceptions;

public class UnexpectedBankPaymentDetailException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.UnexpectedBankPaymentDetail;
}
