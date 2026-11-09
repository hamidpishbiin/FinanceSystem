namespace FinanceSystem.Domain.Payments.Enums;

public enum PaymentFailureReason
{
    CallbackFailed = 1,
    VerificationFailed = 2,
    TokenRequestFailed = 3,
    CallbackAmountMismatch = 4
}
