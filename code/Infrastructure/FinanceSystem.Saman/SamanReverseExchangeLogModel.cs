namespace FinanceSystem.Saman;

public class SamanReverseExchangeLogModel : SamanExchangeLogModel
{
    public string RequestReferenceNumber { get; set; } = string.Empty;
    public SamanPaymentVerificationResponse? Response { get; set; }
    public string? RawResponseBody { get; set; }
}
