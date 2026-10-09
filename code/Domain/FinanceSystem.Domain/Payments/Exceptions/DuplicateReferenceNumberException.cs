namespace FinanceSystem.Domain.Payments.Exceptions;

public class DuplicateReferenceNumberException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.DuplicateReferenceNumber;
}
