namespace FinanceSystem.Domain.Payments.Exceptions;

public class UnexpectedPspCodeException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.UnexpectedPspCode;
}
