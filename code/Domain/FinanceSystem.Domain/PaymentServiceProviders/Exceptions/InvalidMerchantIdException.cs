namespace FinanceSystem.Domain.PaymentServiceProviders.Exceptions;

public class InvalidMerchantIdException : BusinessException
{
    protected override int DefaultCode => PaymentServiceProviderExceptionCodes.InvalidMerchantId;
}
