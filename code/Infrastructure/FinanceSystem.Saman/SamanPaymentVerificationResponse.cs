using FinanceSystem.Application.Payments.Gateways;

namespace FinanceSystem.Saman;

internal class SamanPaymentVerificationResponse
{
    public TransactionDetail? TransactionDetail { get; set; }
    public int? ResultCode { get; set; }
    public string? ResultDescription { get; set; }
    public bool? Success { get; set; }
    public PspTransactionType PspTransactionType { get; set; } = PspTransactionType.Verification;
}

internal class TransactionDetail
{
    public string? RRN { get; set; }
    public string? RefNum { get; set; }
    public string? MaskedPan { get; set; }
    public string? HashedPan { get; set; }
    public int? TerminalNumber { get; set; }
    public int? OrginalAmount { get; set; }
    public int? AffectiveAmount { get; set; }
    public string? StraceDate { get; set; }
    public string? StraceNo { get; set; }
}
