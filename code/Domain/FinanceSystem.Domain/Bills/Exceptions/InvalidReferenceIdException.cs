namespace FinanceSystem.Domain.Bills.Exceptions;

public class InvalidReferenceIdException : BusinessException
{
    override protected int DefaultCode => BillExceptionCodes.InvalidReferenceId;
}
