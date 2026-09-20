namespace FinanceSystem.Domain.PspPaymentDetails.Exceptions;

public class InvalidTokenException : BusinessException
{
    protected override int DefaultCode => PspPaymentDetailExceptionCodes.InvalidToken;
}
