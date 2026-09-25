namespace FinanceSystem.Domain.Payments.Exceptions;

public class MissingRequestToPayException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.MissingRequestToPay;
}
