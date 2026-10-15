namespace FinanceSystem.Domain.Bills.Exceptions;

public class InvalidServiceIdException : BusinessException
{
    override protected int DefaultCode => BillExceptionCodes.InvalidServiceId;
}
