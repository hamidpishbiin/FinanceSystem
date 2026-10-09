namespace FinanceSystem.Interface.Contracts.Payments.Models;

public class SepCallbackModel
{
    public string? Token { get; set; }
    public uint? MID { get; set; }
    public string? State { get; set; }
    public int? Status { get; set; }
    public string? RRN { get; set; }
    public string? RefNum { get; set; }
    public string? ResNum { get; set; }
    public uint? TerminalId { get; set; }
    public string? TraceNo { get; set; }
    public uint? Amount { get; set; }
    public string? SecurePan { get; set; }
    public string? Wage { get; set; }
    public string? HashedCardNumber { get; set; }
}
