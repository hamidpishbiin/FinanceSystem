namespace FinanceSystem.Application.Contracts.Payments.Command;

public class PayWithPspCommand : PaymentCommand
{
    public int PspCode { get; set; }
}
