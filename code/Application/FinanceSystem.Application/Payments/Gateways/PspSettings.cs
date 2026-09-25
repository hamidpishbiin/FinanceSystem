namespace FinanceSystem.Application.Payments.Gateways;

public class PspSettings
{
    public string TopUpCallBackUrl { get; set; } = default!;
    public string BaseUrl { get; set; } = default!;
    public string MerchantId { get; set; } = default!;
    public string TerminalId { get; set; } = default!;
    public string CredentialsRef { get; set; } = default!;
    public int TimeoutInSeconds { get; set; }
}
