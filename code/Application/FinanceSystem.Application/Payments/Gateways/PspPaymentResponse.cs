using FinanceSystem.Domain.PaymentServiceProviders.Enums;

namespace FinanceSystem.Application.Payments.Gateways;

public class PspPaymentResponse
{
    public PspCode PspCode { get; set; }
    public bool IsSuccess { get; set; }
    public string PaymentToken { get; set; } = string.Empty;
    public string IpgUrl { get; set; } = string.Empty;
    public string ResultCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Dictionary<string, string> ExtraData { get; set; } = [];
    public string CallbackUrl { get; set; } = string.Empty;
}
