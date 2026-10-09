using FinanceSystem.Domain.Payments.Enums;

namespace FinanceSystem.Application.Payments.Gateways;

public class PspTokenResponse
{
    public bool IsSuccess { get; set; }
    public string PaymentToken { get; set; } = string.Empty;
    public string IpgUrl { get; set; } = string.Empty;
    public PspFailureReason? FailureReason { get; set; }
    public string RawStatus { get; set; } = string.Empty;
    public int RawStatusCode { get; set; }
    public string RawDescription { get; set; } = string.Empty;
}
