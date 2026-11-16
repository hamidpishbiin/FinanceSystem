namespace FinanceSystem.Saman;

public class SamanCallbackLogModel : SamanLogModel
{
    public string RawFields { get; set; } = string.Empty;
    public int? Status { get; set; }
    public long? ResNum { get; set; }
    public string? Token { get; set; }
    public string? RefNum { get; set; }
    public decimal? Amount { get; set; }
    public string? SecurePan { get; set; }
    public string? RRN { get; set; }
    public string? TraceNo { get; set; }
}
