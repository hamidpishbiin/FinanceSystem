namespace FinanceSystem.Domain.Payments.Exceptions;

public class CallbackTokenMismatchException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.CallbackTokenMismatch;
}
