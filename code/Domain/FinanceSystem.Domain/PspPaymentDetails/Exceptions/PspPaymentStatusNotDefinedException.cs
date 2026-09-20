namespace FinanceSystem.Domain.PspPaymentDetails.Exceptions;

public class PspPaymentStatusNotDefinedException : BusinessException
{
    protected override int DefaultCode => PspPaymentDetailExceptionCodes.PspPaymentStatusNotDefined;
}
