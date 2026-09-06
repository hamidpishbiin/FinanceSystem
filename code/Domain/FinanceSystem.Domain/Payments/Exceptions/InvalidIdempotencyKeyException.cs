namespace FinanceSystem.Domain.Payments.Exceptions;

public class InvalidIdempotencyKeyException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.InvalidIdempotencyKey;
}
