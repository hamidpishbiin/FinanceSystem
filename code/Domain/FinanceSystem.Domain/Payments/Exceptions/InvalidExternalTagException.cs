namespace FinanceSystem.Domain.Payments.Exceptions;

public class InvalidExternalTagException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.InvalidExternalTag;
}
