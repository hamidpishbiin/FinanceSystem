namespace FinanceSystem.Domain.FinanceAccounts.Exceptions;

public static class FinanceAccountExceptionCodes
{
    public const int InvalidUserId = 300;
    public const int EntryFinanceAccountMismatch = 301;
    public const int FinanceAccountDirectionNotAllowed = 303;
    public const int InsufficientBalance = 304;
    public const int FinanceAccountNotFound = 306;
    public const int InvalidFinanceAccountId = 307;
    public const int InvalidFinanceAccountState = 308;
    public const int ValidTopUpAccountNotFound = 309;
    public const int MoreThanOneValidTopUpAccountFound = 310;
    public const int ValidWalletPurchaseAccountNotFound = 311;
    public const int MoreThanOneValidWalletPurchaseAccountFound = 312;
}
