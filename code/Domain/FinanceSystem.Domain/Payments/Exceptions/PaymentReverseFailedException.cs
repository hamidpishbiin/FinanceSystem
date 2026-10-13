namespace FinanceSystem.Domain.Payments.Exceptions;

public class PaymentReverseFailedException(int code, string message) : BusinessException(code, message)
{

}
