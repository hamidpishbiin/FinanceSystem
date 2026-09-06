namespace FinanceSystem.Domain.Payments.Exceptions;

public class SameSourceAndDestinationAccountException : BusinessException
{
    protected override int DefaultCode => PaymentExceptionCodes.SameSourceAndDestinationAccount;
}
