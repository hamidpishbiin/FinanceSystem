namespace FinanceSystem.Interface.Contracts.Payments.Models;

public class PaymentOptionsRequestModel
{
    public required string Token { get; set; }
    public long? UserId { get; set; }
}
