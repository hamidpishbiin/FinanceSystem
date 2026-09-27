namespace FinanceSystem.Domain.Payments.Exceptions;

public class MissingPspCodeException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.MissingPspCode;
}
