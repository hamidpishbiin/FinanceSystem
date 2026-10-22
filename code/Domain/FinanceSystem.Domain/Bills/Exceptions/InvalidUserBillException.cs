namespace FinanceSystem.Domain.Bills.Exceptions;

public class InvalidUserBillException : BusinessException
{
    override protected int DefaultCode => BillExceptionCodes.InvalidUserBill;
}
