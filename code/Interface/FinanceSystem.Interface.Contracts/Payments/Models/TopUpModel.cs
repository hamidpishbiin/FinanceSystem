namespace FinanceSystem.Interface.Contracts.Payments.Models;

public class TopUpModel
{
    public required decimal Amount { get; set; }
    public int? PspCode { get; set; }
}
