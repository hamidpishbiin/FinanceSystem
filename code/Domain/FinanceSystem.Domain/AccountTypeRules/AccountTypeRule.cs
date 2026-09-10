using FinanceSystem.Domain.AccountTypeRules.Exceptions;
using FinanceSystem.Domain.Accounts.Enums;
using FinanceSystem.Domain.Payments.Enums;

namespace FinanceSystem.Domain.AccountTypeRules;

public class AccountTypeRule : EntityBase<int>
{
    public AccountType Type { get; private set; }
    public PaymentPurpose Purpose { get; private set; }
    public bool CanBeSource { get; private set; }
    public bool CanBeDestination { get; private set; }

    private AccountTypeRule()
    {
    }

    public static async Task<AccountTypeRule> Create(
        AccountType type,
        PaymentPurpose purpose,
        bool canBeSource,
        bool canBeDestination)
    {
        Guard<InvalidAccountTypeException>.IsFalse(Enum.IsDefined(type));
        Guard<InvalidPaymentPurposeException>.IsFalse(Enum.IsDefined(purpose));
        Guard<InvalidSourceDestinationException>.IsTrue(!canBeSource && !canBeDestination);

        return new AccountTypeRule()
        {
            Type = type,
            Purpose = purpose,
            CanBeSource = canBeSource,
            CanBeDestination = canBeDestination
        };
    }
}
