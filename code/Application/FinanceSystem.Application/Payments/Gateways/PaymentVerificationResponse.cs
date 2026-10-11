namespace FinanceSystem.Application.Payments.Gateways;

public class PaymentVerificationResponse
{
    public bool HasResponse { get; set; }
    public bool IsSuccess { get; set; }
    public decimal? Amount { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public PspTransactionType PspTransactionType { get; set; }
    public object? RawData { get; set; }
}
