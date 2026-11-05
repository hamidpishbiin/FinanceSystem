namespace FinanceSystem.Application.Contracts.Payments.Command;

public class PspPaymentCommand : PaymentCommand
{
    public required long BillId { get; set; }
    public int PspCode { get; set; }
}
