using FinanceSystem.Domain.FinanceAccounts.Enums;
using FinanceSystem.Domain.AccountTypeRules;
using FinanceSystem.Domain.Payments.Enums;

namespace FinanceSystem.Domain.Tests.Unit.AccountTypeRuleUnitTests;

public class AccountTypeRuleBuilder
{
    public const FinanceAccountType DefaultType = FinanceAccountType.UserWallet;
    public const PaymentPurpose DefaultPaymentPurpose = PaymentPurpose.TopUp;
    public const bool DefaultCanBeSource = true;
    public const bool DefaultCanBeDestination = true;

    private FinanceAccountType Type { get; set; } = DefaultType;
    private PaymentPurpose Purpose { get; set; } = DefaultPaymentPurpose;
    private bool CanBeSource { get; set; } = DefaultCanBeSource;
    private bool CanBeDestination { get; set; } = DefaultCanBeDestination;

    public AccountTypeRule Build()
    {
        return new AccountTypeRule(Type, Purpose, CanBeSource, CanBeDestination);
    }

    public AccountTypeRuleBuilder WithType(FinanceAccountType type)
    {
        Type = type;
        return this;
    }

    public AccountTypeRuleBuilder WithPurpose(PaymentPurpose purpose)
    {
        Purpose = purpose;
        return this;
    }

    public AccountTypeRuleBuilder WithCanBeSource(bool canBeSource)
    {
        CanBeSource = canBeSource;
        return this;
    }

    public AccountTypeRuleBuilder WithCanBeDestination(bool canBeDestination)
    {
        CanBeDestination = canBeDestination;
        return this;
    }
}
