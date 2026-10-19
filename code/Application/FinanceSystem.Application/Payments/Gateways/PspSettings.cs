namespace FinanceSystem.Application.Payments.Gateways;

public class PspSettings
{
    public string TopUpCallBackUrl { get; set; } = string.Empty;
    public string TopUpResultPageUrl { get; set; } = string.Empty;
    public string PaymentCallBackUrl { get; set; } = string.Empty;
    public string PaymentResultPageUrl { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public string MerchantId { get; set; } = string.Empty;
    public string TerminalId { get; set; } = string.Empty;
    public string CredentialsRef { get; set; } = string.Empty;
    public int TimeoutInSeconds { get; set; }
}
