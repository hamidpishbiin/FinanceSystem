namespace FinanceSystem.Application.Contracts.Payments.Command;

public class PspPaymentCommand : PaymentCommand
{
    public required long Token { get; set; }
    public int PspCode { get; set; }
}
