namespace FinanceSystem.Domain.PspPaymentDetails.Exceptions;

public class InvalidRequestAmountException : BusinessException
{
    protected override int DefaultCode => PspPaymentDetailExceptionCodes.InvalidRequestAmount;
}
