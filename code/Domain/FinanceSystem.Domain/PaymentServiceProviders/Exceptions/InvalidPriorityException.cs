namespace FinanceSystem.Domain.PaymentServiceProviders.Exceptions;

public class InvalidPriorityException : BusinessException
{
    protected override int DefaultCode => PaymentServiceProviderExceptionCodes.InvalidPriority;
}
