namespace FinanceSystem.Interface.Contracts.TopUps.Models;

public class TopUpModel
{
    public required decimal Amount { get; set; }
    public int? PspCode { get; set; }
}
