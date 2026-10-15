namespace FinanceSystem.Domain.Bills.Exceptions;

public class InvalidAmountException : BusinessException
{
    override protected int DefaultCode => BillExceptionCodes.InvalidAmount;
}
