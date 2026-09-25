using FinanceSystem.Domain.PspPaymentDetails.Enums;

namespace FinanceSystem.Application.Payments.Gateways;

public class PspPaymentResponse
{
    public bool IsSuccess { get; set; }
    public string PaymentToken { get; set; } = string.Empty;
    public string IpgUrl { get; set; } = string.Empty;
    public PspFailureReason? FailureReason { get; set; }
    public string RawStatus { get; set; } = string.Empty;
    public string RawErrorCode { get; set; } = string.Empty;
    public string RawDescription { get; set; } = string.Empty;
}
