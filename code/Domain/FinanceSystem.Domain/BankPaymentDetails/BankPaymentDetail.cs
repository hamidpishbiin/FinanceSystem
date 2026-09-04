namespace FinanceSystem.Domain.BankPaymentDetails;

public class BankPaymentDetail : EntityBase<long>, IAggregateRoot
{
    public IEventPublisher Publisher { get; set; }

    public Guid PspId { get; set; }
    public BankPaymentStatus Status { get; set; }
    public decimal RequestAmountRial { get; set; }
    public decimal RedirectedAmountRial { get; set; }
    public long TargetAccountId { get; set; }
    public string Authority { get; set; }
    public string? RRN { get; set; }
    public string? RefNum { get; set; }
    public string? TraceNumber { get; set; }
    public string? MaskedPan { get; set; }
    public short? ResultCode { get; set; }
    public string? RawCallback { get; set; }
    public DateTimeOffset? VerifiedAtUtc { get; set; }
}
