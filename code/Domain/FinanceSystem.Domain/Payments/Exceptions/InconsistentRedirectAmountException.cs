namespace FinanceSystem.Domain.Payments.Exceptions;

public class InconsistentRedirectAmountException : BusinessException
{
    override protected int DefaultCode => PaymentExceptionCodes.InconsistentRedirectAmount;
}
