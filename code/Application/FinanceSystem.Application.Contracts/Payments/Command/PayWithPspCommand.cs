namespace FinanceSystem.Application.Contracts.Payments.Command;

public class PayWithPspCommand : PaymentCommand
{
    public required long Token { get; set; }
    public int PspCode { get; set; }
}
