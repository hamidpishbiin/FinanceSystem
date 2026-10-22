namespace FinanceSystem.Domain.FinanceAccounts.Exceptions;

public class ValidWalletPurchaseAccountNotFoundException : BusinessException
{
    override protected int DefaultCode => FinanceAccountExceptionCodes.ValidWalletPurchaseAccountNotFound;
}
