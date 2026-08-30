using ProductManagement.Domain.Accounts;
using ProductManagement.Domain.Payments.Enums;

namespace ProductManagement.Domain.AccountTypeRules;

public class AccountTypeRule : EntityBase<int>
{
    public AccountType Type { get; set; }
    public PaymentPurpose Purpose { get; set; }
    public bool CanBeSource { get; set; }
    public bool CanBeDestination { get; set; }
}
