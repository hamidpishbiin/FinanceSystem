namespace FinanceSystem.Domain.Payments.Enums;

public enum PaymentFailureReason
{
    Unknown = 0,
    DuplicateReferenceNumber = 1,
    CallbackAmountMismatch = 2,
    VerificationFailed = 3,
    VerificationAmountMismatch = 4,
}
