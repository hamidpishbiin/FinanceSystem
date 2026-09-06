namespace FinanceSystem.Domain.BankPaymentDetails.Exceptions;

public class InvalidTargetAccountIdException : BusinessException
{
    protected override int DefaultCode => BankPaymentDetailExceptionCodes.InvalidTargetAccountId;
}
