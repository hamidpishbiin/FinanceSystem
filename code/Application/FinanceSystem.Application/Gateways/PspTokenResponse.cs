using FinanceSystem.Application.Contracts.Payments;

namespace FinanceSystem.Application.Gateways;

public class PspTokenResponse
{
    public bool IsSuccess { get; set; }
    public string Token { get; set; } = string.Empty;
    public PspHandoff? Handoff { get; set; }
}
