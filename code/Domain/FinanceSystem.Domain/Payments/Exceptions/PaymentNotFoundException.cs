namespace FinanceSystem.Domain.Payments.Exceptions;

public class PaymentNotFoundException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.PaymentNotFound;
}
