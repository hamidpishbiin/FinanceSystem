namespace FinanceSystem.Domain.Payments.Exceptions;

public class InvalidReferenceIdException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.InvalidReferenceId;
}
