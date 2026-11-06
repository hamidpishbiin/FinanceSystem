namespace FinanceSystem.Interface.Contracts.Bills.Models;

public class PspPaymentRequest
{
    public required long BillId { get; set; }
    public required int PspCodeValue { get; set; }
}
