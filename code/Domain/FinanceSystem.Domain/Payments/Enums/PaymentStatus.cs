namespace FinanceSystem.Domain.Payments.Enums;

public enum PaymentStatus : byte
{
	Pending = 1,
	Succeeded = 2,
	Failed = 3,
	Reversed = 4
}