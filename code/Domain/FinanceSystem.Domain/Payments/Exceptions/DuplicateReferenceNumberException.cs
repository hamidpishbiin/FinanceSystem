namespace FinanceSystem.Domain.Payments.Exceptions;

public class DuplicateReferenceNumberException : BusinessException
{
    override protected int DefaultCode => PaymentExceptionCodes.DuplicateReferenceNumber;
}
