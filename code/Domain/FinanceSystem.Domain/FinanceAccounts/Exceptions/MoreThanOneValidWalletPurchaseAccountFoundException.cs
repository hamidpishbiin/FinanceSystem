namespace FinanceSystem.Domain.FinanceAccounts.Exceptions;

public class MoreThanOneValidWalletPurchaseAccountFoundException : BusinessException
{
    override protected int DefaultCode => FinanceAccountExceptionCodes.MoreThanOneValidWalletPurchaseAccountFound;
}
