namespace FinanceSystem.Domain.Payments.Exceptions;

public class UnexpectedRequestToPayException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.UnexpectedRequestToPay;
}
