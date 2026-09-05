namespace FinanceSystem.Domain.Accounts.Exceptions;

public static class AccountExceptionCodes
{
    public const int InvalidOwnerId = 300;
    public const int EntryAccountMismatch = 301;
    public const int AccountClosed = 302;
    public const int AccountDirectionNotAllowed = 303;
    public const int InsufficientBalance = 304;
    public const int NullEntry = 305;
}
