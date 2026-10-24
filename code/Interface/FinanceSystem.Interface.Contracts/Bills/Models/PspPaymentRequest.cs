namespace FinanceSystem.Interface.Contracts.Bills.Models;

public class PspPaymentRequest
{
    public required string BillId { get; set; }
    public required int PspCode { get; set; }
}
