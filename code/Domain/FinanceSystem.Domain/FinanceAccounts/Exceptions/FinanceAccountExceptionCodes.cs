namespace FinanceSystem.Domain.FinanceAccounts.Exceptions;

public static class FinanceAccountExceptionCodes
{
    public const int InvalidUserId = 300;
    public const int EntryFinanceAccountMismatch = 301;
    public const int FinanceAccountClosed = 302;
    public const int FinanceAccountDirectionNotAllowed = 303;
    public const int InsufficientBalance = 304;
}
