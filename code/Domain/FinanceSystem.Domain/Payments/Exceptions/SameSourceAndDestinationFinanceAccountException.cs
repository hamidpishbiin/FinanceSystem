namespace FinanceSystem.Domain.Payments.Exceptions;

public class SameSourceAndDestinationFinanceAccountException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.SameSourceAndDestinationFinanceAccount;
}
