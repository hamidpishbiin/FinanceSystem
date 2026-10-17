namespace FinanceSystem.Domain.Bills.Exceptions;

public class BillNotFoundException : BusinessException
{
    override protected int DefaultCode => BillExceptionCodes.BillNotFound;
}
