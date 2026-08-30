namespace ProductManagement.Domain.Payments.Exceptions
{
    public class InvalidCurrencyException() : BusinessException(PaymentExceptionCodes.InvalidCurrencyExceptionCode)
    {

    }
}
