namespace FinanceSystem.Domain.BankPaymentDetails.Exceptions;

public class BankPaymentStatusNotDefinedException : BusinessException
{
    protected override int DefaultCode => BankPaymentDetailExceptionCodes.BankPaymentStatusNotDefined;
}
