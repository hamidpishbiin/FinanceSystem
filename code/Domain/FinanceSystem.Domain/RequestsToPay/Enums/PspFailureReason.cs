namespace FinanceSystem.Domain.RequestsToPay.Enums;

public enum PspFailureReason
{
    None = 0,
    InsufficientFunds = 1,
    InvalidTerminal = 2,
    InvalidAmount = 3,
    DuplicateReference = 4,
    Timeout = 5,
    Rejected = 6,
    Unknown = 7,
    HttpException = 8,
}
