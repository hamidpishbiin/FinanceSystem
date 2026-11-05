namespace FinanceSystem.Application.Gateways;

public class PspTokenResponse
{
    public bool IsSuccess { get; set; }
    public string Token { get; set; } = string.Empty;
    public int Status { get; set; }
    public int StatusCode { get; set; }
    public string Description { get; set; } = string.Empty;
}
