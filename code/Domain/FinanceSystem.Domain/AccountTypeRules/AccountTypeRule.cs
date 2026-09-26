using FinanceSystem.Domain.AccountTypeRules.Exceptions;
using FinanceSystem.Domain.FinanceAccounts.Enums;
using FinanceSystem.Domain.Payments.Enums;

namespace FinanceSystem.Domain.AccountTypeRules;

public class AccountTypeRule : EntityBase<int>
{
    public FinanceAccountType Type { get; private set; }
    public PaymentPurpose Purpose { get; private set; }
    public bool CanBeSource { get; private set; }
    public bool CanBeDestination { get; private set; }

    private AccountTypeRule()
    {
    }

    public AccountTypeRule(
        FinanceAccountType type,
        PaymentPurpose purpose,
        bool canBeSource,
        bool canBeDestination)
    {
        Guard<InvalidFinanceAccountTypeException>.IsFalse(Enum.IsDefined(type));
        Guard<InvalidPaymentPurposeException>.IsFalse(Enum.IsDefined(purpose));
        Guard<InvalidSourceDestinationException>.IsTrue(!canBeSource && !canBeDestination);

        Type = type;
        Purpose = purpose;
        CanBeSource = canBeSource;
        CanBeDestination = canBeDestination;
    }
}
