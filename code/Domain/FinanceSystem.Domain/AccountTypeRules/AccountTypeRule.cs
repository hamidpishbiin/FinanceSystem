using FinanceSystem.Domain.Payments.Exceptions;
using FinanceSystem.Domain.AccountTypeRules.Exceptions;
using FinanceSystem.Domain.FinanceAccounts.Enums;
using FinanceSystem.Domain.FinanceAccounts.Exceptions;
using FinanceSystem.Domain.Payments.Enums;

namespace FinanceSystem.Domain.AccountTypeRules;

public class AccountTypeRule : EntityBase<int>
{
    public FinanceAccountType Type { get; private set; }
    public FinanceAccountState State { get; set; }
    public PaymentPurpose Purpose { get; private set; }
    public bool IsAllowed { get; private set; }

    private AccountTypeRule()
    {
    }

    public AccountTypeRule(
        FinanceAccountType type,
        FinanceAccountState state,
        PaymentPurpose purpose,
        bool isAllowed)
    {
        Guard<InvalidFinanceAccountTypeException>.IsFalse(Enum.IsDefined(type));
        Guard<InvalidFinanceAccountStateException>.IsFalse(Enum.IsDefined(state));
        Guard<InvalidPaymentPurposeException>.IsFalse(Enum.IsDefined(purpose));

        Type = type;
        State = state;
        Purpose = purpose;
        IsAllowed = isAllowed;
    }
}
