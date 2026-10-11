namespace FinanceSystem.Domain.Payments.Enums;

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
    CanceledByUser = 9,
    AmountMismatch = 10,
    VerificationFailed = 11,
    SessionExpired = 12,
    InvalidRequest = 13,
    MerchantConfigurationError = 14,
}
