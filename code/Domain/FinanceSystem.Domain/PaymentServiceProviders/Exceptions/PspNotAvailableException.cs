namespace FinanceSystem.Domain.PaymentServiceProviders.Exceptions;

public class PspNotAvailableException : BusinessException
{
    protected override int DefaultCode => PspExceptionCodes.PspNotAvailable;
}
