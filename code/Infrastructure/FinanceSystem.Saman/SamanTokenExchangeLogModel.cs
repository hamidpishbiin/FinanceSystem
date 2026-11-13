namespace FinanceSystem.Saman;

public class SamanTokenExchangeLogModel
{
    public long Id { get; private set; }
    public decimal RequestAmount { get; set; }
    public long RequestReservationNumber { get; set; }
    public SamanTokenResponse? Response { get; set; }
    public string? RawResponseBody { get; set; } = string.Empty;
}
