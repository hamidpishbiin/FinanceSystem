namespace ProductManagement.Domain.BalanceCheckpoints;

public class BalanceCheckpoint : EntityBase<long>
{
    public long AccountId { get; set; }
    public long UpToEntryId { get; set; }
    public decimal BalanceRial { get; set; }
}
