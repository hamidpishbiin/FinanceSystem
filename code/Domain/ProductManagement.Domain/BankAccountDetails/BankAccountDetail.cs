namespace ProductManagement.Domain.BankAccountDetails;

public class BankAccountDetail : EntityBase<long>
{
    public long AccountId { get; set; }
    public string Iban { get; set; }
    public string? MaskedPan { get; set; }
    public string? BankName { get; set; }
}
