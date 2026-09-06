namespace FinanceSystem.Domain.BankPaymentDetails.Exceptions;

public class InvalidAuthorityException : BusinessException
{
    protected override int DefaultCode => BankPaymentDetailExceptionCodes.InvalidAuthority;
}
