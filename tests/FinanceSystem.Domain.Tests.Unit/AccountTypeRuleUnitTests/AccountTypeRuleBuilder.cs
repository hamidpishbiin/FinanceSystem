using FinanceSystem.Domain.Accounts.Enums;
using FinanceSystem.Domain.AccountTypeRules;
using FinanceSystem.Domain.Payments.Enums;

namespace FinanceSystem.Domain.Tests.Unit.AccountTypeRuleUnitTests;

public class AccountTypeRuleBuilder
{
    public const AccountType DefaultType = AccountType.BankGateway;
    public const PaymentPurpose DefaultPaymentPurpose = PaymentPurpose.TopUp;
    public const bool DefaultCanBeSource = true;
    public const bool DefaultCanBeDestination = true;

    private AccountType Type { get; set; } = DefaultType;
    private PaymentPurpose Purpose { get; set; } = DefaultPaymentPurpose;
    private bool CanBeSource { get; set; } = DefaultCanBeSource;
    private bool CanBeDestination { get; set; } = DefaultCanBeDestination;

    public async Task<AccountTypeRule> Build()
    {
        return await AccountTypeRule.Create(Type, Purpose, CanBeSource, CanBeDestination);
    }

    public AccountTypeRuleBuilder WithType(AccountType type)
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
