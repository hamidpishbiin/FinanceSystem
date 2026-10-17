using FinanceSystem.Domain.Payments.Enums;

namespace FinanceSystem.Application.Payments.Gateways;

public class PspTokenResponse
{
    public bool IsSuccess { get; set; }
    public string Token { get; set; } = string.Empty;
    public string IpgUrl { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public string Description { get; set; } = string.Empty;
}
