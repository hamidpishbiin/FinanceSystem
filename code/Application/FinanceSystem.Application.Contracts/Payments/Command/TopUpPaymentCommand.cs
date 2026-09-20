namespace FinanceSystem.Application.Contracts.Payments.Command;

public class TopUpPaymentCommand : PaymentCommand
{
    public decimal AmountRial { get; set; }
    public int PspCode { get; set; }
    public Guid UserId { get; set; }
}
