namespace FinanceSystem.Domain.BankPaymentDetails;

public enum BankPaymentStatus
{
    Initiated = 1,
    CallbackReceived = 2,
    Verified = 3,
    Failed = 4,
    Expired = 5,
    Reversed = 6
}
