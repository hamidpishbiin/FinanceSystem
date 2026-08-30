namespace ProductManagement.Domain.AccountEntries;

public class AccountEntry
{
    public long PaymentId { get; set; }
    public long AccountId { get; set; }
    public decimal AmountRial { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}
