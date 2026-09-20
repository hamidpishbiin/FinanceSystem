namespace FinanceSystem.Domain.PspPaymentDetails.Exceptions;

public class InvalidTargetAccountIdException : BusinessException
{
    protected override int DefaultCode => PspPaymentDetailExceptionCodes.InvalidTargetAccountId;
}
