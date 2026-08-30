namespace ProductManagement.Domain.Payments.Exceptions;

public class IncoherentPaymentException() : BusinessException(PaymentExceptionCodes.IncoherentPaymentExceptionCode)
{

}
