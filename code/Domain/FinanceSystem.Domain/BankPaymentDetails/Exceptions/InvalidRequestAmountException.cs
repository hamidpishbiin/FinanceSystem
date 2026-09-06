namespace FinanceSystem.Domain.BankPaymentDetails.Exceptions;

public class InvalidRequestAmountException : BusinessException
{
    protected override int DefaultCode => BankPaymentDetailExceptionCodes.InvalidRequestAmount;
}
