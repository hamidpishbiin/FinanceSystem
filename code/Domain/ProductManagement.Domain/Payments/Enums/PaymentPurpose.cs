namespace ProductManagement.Domain.Payments.Enums;

public enum PaymentPurpose : byte
{
	TopUp = 1,
	Purchase = 2,
	Withdrawal = 3,
	Refund = 4,
	Transfer = 5,
	Adjustment = 6
}