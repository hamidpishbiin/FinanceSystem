namespace FinanceSystem.Domain.RequestsToPay.Exceptions;

public class RequestToPayStatusNotDefinedException : BusinessException
{
    protected override int DefaultCode => RequestToPayExceptionCodes.RequestToPayStatusNotDefined;
}
