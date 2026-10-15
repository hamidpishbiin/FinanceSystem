namespace FinanceSystem.Application.Contracts.Payments.Command;

public class TopUpCommand : PaymentCommand
{
    public decimal Amount { get; set; }
    public int? PspCode { get; set; }
    public long UserId { get; set; }
}
