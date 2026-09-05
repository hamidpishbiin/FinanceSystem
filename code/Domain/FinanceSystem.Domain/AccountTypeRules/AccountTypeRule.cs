using FinanceSystem.Domain.Accounts;
using FinanceSystem.Domain.Accounts.Enums;
using FinanceSystem.Domain.Payments.Enums;

namespace FinanceSystem.Domain.AccountTypeRules;

public class AccountTypeRule : EntityBase<int>
{
    public AccountType Type { get; set; }
    public PaymentPurpose Purpose { get; set; }
    public bool CanBeSource { get; set; }
    public bool CanBeDestination { get; set; }
}
