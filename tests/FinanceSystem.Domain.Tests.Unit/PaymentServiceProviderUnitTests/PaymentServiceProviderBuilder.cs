using FinanceSystem.Domain.PaymentServiceProviders;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;

namespace FinanceSystem.Domain.Tests.Unit.PaymentServiceProviderUnitTests;

public class PaymentServiceProviderBuilder
{
    public const PspCode DefaultCode = PspCode.Saman;
    public const string DefaultName = "Saman Bank Gateway";
    public const bool DefaultIsActive = true;
    public const short DefaultPriority = 10;

    private PspCode Code { get; set; } = DefaultCode;
    private string Name { get; set; } = DefaultName;
    private bool IsActive { get; set; } = DefaultIsActive;
    private short Priority { get; set; } = DefaultPriority;

    public PaymentServiceProvider Build()
    {
        return new PaymentServiceProvider(
            Code,
            Name,
            IsActive,
            Priority);
    }

    public PaymentServiceProviderBuilder WithCode(PspCode code)
    {
        Code = code;
        return this;
    }

    public PaymentServiceProviderBuilder WithName(string? name)
    {
        Name = name!;
        return this;
    }

    public PaymentServiceProviderBuilder WithIsActive(bool isActive)
    {
        IsActive = isActive;
        return this;
    }

    public PaymentServiceProviderBuilder WithPriority(short priority)
    {
        Priority = priority;
        return this;
    }




}
