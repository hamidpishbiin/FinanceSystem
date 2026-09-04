namespace FinanceSystem.Domain.Payments.Exceptions;

public class IncoherentPaymentException() : BusinessException(PaymentExceptionCodes.IncoherentPaymentExceptionCode)
{

}
