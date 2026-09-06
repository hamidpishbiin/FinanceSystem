namespace FinanceSystem.Domain.BankPaymentDetails.Exceptions;

public class InvalidPspIdException : BusinessException
{
    protected override int DefaultCode => BankPaymentDetailExceptionCodes.InvalidPspId;
}
