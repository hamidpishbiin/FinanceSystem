namespace FinanceSystem.Interface.Contracts.Payments.Models;

public class BasePaymentModel
{
    public required string Token { get; set; }
    public long? UserId { get; set; }
}
