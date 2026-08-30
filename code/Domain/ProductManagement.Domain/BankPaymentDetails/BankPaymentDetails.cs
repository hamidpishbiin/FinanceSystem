namespace ProductManagement.Domain.BankPaymentDetails;

public class BankPaymentDetails : EntityBase<long>, IAggregateRoot
{

    public IEventPublisher Publisher { get; set; }

    public long Id { get; set; }
    public short PspId { get; set; }
    public BankPaymentStatus Status { get; set; }
    public long RequestAmountRial { get; set; }
    public long TargetAccountId { get; set; }
    public string Authority { get; set; }
    public string RRN { get; set; }
    public string RefId { get; set; }
    public string TraceNumber { get; set; }
    public string MaskedPan { get; set; }
    public short ResultCode { get; set; }
    public string RawCallback { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? VerifiedAtUtc { get; set; }
}
