namespace FinanceSystem.Application.Payments.Gateways;

// decimal Amount, long ReferenceNumber, string CallbackUrl, string AdditionalData
public class PspPaymentRequest()
{
    public required decimal Amount { get; set; }
    public required long ReferenceNumber { get; set; }
    public required string CallbackUrl { get; set; }
    public string? AdditionalData { get; set; }
}
