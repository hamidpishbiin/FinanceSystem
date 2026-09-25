namespace FinanceSystem.Application.Payments.Gateways;

// decimal Amount, long ReferenceNumber, string AdditionalData
public class PspTokenRequest()
{
    public required decimal Amount { get; set; }
    public required long ReferenceNumber { get; set; }
    public string? AdditionalData { get; set; }
}
