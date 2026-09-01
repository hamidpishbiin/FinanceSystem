namespace ProductManagement.Domain.Accounts;

public class Account : EntityBase<long>
{
    public AccountType Type { get; set; }
    public AccountStatus Status { get; set; }
    public long OwnerId { get; set; }
    public decimal CachedBalanceRial { get; set; }
    public DateTimeOffset BalanceCalculatedAt { get; set; }
    public bool AllowNegativeBalance { get; set; }
    public uint RowVersion { get; set; }
}
