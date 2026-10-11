namespace FinanceSystem.Domain.Payments.Enums;

public enum PaymentState : byte
{
    Initiated = 1,
    TokenReceived = 2,
    CallbackReceived = 3,
    Succeeded = 4,
    Failed = 5,
    Expired = 6,
    Reversed = 7,
    Verifying = 8
}
