namespace FinanceSystem.Interface.Contracts.Payments.Models;

public class TopUpModel
{
    public required decimal Amount { get; set; }
    public required int PspCode { get; set; }
    public required Guid UserId { get; set; }
}
