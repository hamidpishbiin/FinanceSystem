namespace FinanceSystem.Domain.AccountEntries;

public class AccountEntry : EntityBase<long>
{
    public long PaymentId { get; set; }
    public long AccountId { get; set; }
    public decimal AmountRial { get; set; }
}
