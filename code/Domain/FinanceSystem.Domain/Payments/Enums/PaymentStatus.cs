namespace FinanceSystem.Domain.Payments.Enums;

public enum PaymentStatus : byte
{
    Initiated = 1,
    TokenReceived = 2,
    CallbackReceived = 3,
    Verified = 4,
    Failed = 5,
    Expired = 6,
    Reversed = 7
}
