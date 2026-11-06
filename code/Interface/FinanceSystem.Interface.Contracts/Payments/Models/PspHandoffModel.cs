namespace FinanceSystem.Interface.Contracts.Payments.Models;

public class PspHandoffModel
{
    public required string Url { get; set; }
    public required string Method { get; set; }
    public required IReadOnlyDictionary<string, string> Fields { get; set; }
}
