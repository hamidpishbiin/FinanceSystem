namespace FinanceSystem.Domain.Payments.Exceptions;

public class UnexpectedPspPaymentDetailException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.UnexpectedPspPaymentDetail;
}
