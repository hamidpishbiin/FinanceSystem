namespace FinanceSystem.Domain.Payments.Exceptions;

public class InvalidPaymentChannelException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.InvalidPaymentChannel;
}
