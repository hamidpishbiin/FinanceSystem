namespace FinanceSystem.Domain.Payments.Exceptions;

public class PspCodeMismatchException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.PspCodeMismatch;
}
