using FinanceSystem.Domain.FinanceAccounts.Enums;
using FinanceSystem.Domain.AccountTypeRules;
using FinanceSystem.Domain.Payments.Enums;

namespace FinanceSystem.Domain.Tests.Unit.AccountTypeRuleUnitTests;

public class AccountTypeRuleBuilder
{
    public const FinanceAccountType DefaultType = FinanceAccountType.UserWallet;
    public const FinanceAccountState DefaultSate = FinanceAccountState.Active;
    public const PaymentPurpose DefaultPaymentPurpose = PaymentPurpose.TopUp;
    public const bool DefaultIsAllowed = true;
    public const bool DefaultCanBeDestination = true;

    private FinanceAccountType Type { get; set; } = DefaultType;
    private FinanceAccountState State { get; set; } = DefaultSate;
    private PaymentPurpose Purpose { get; set; } = DefaultPaymentPurpose;
    private bool IsAllowed { get; set; } = DefaultIsAllowed;

    public AccountTypeRule Build()
    {
        return new AccountTypeRule(Type, State, Purpose, IsAllowed);
    }

    public AccountTypeRuleBuilder WithType(FinanceAccountType type)
    {
        Type = type;
        return this;
    }

    public AccountTypeRuleBuilder WithState(FinanceAccountState state)
    {
        State = state;
        return this;
    }

    public AccountTypeRuleBuilder WithPurpose(PaymentPurpose purpose)
    {
        Purpose = purpose;
        return this;
    }

    public AccountTypeRuleBuilder WithIsAllowed(bool isAllowed)
    {
        IsAllowed = isAllowed;
        return this;
    }
}
