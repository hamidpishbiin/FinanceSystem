namespace FinanceSystem.Domain.Payments.Exceptions;

public class InvalidExternalReferenceIdException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.InvalidExternalReferenceId;
}
