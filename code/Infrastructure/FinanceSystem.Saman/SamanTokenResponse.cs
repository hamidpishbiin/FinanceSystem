namespace FinanceSystem.Saman;

public class SamanTokenResponse
{
    public int Status { get; set; }
    public string Token { get; set; } = string.Empty;
    public int ErrorCode { get; set; }
    public string ErrorDesc { get; set; } = string.Empty;
}
