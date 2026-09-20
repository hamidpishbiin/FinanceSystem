namespace FinanceSystem.Domain.Payments.Exceptions;

public class MissingPspPaymentDetailException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.MissingPspPaymentDetail;
}
