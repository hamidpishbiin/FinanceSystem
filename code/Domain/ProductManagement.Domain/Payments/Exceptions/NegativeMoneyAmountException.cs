namespace ProductManagement.Domain.Payments.Exceptions
{
    public class NegativeMoneyAmountException() : BusinessException(PaymentExceptionCodes.NegativeMoneyAmountExceptionCode)
    {
    }
}
