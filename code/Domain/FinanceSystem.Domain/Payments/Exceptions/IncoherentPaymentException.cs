namespace FinanceSystem.Domain.Payments.Exceptions;

public class IncoherentPaymentException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.IncoherentPaymentExceptionCode;
}
