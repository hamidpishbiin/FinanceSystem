using FinanceSystem.Domain.PaymentServiceProviders;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;

namespace FinanceSystem.Domain.Tests.Unit.PaymentServiceProviderUnitTests;

public class PaymentServiceProviderBuilder
{
    public const string DefaultIdString = "9f1d2c3b-4a5e-6f70-8192-a3b4c5d6e7f8";
    public const PspCode DefaultCode = PspCode.Saman;
    public const string DefaultName = "Saman Bank Gateway";
    public const bool DefaultIsActive = true;
    public const short DefaultPriority = 10;

    public static readonly Guid DefaultId = Guid.Parse(DefaultIdString);

    private Guid Id { get; set; } = DefaultId;
    private PspCode Code { get; set; } = DefaultCode;
    private string Name { get; set; } = DefaultName;
    private bool IsActive { get; set; } = DefaultIsActive;
    private short Priority { get; set; } = DefaultPriority;

    public async Task<PaymentServiceProvider> Build()
    {
        return await PaymentServiceProvider.Create(
            Id,
            Code,
            Name,
            IsActive,
            Priority);
    }

    public PaymentServiceProviderBuilder WithId(Guid id)
    {
        Id = id;
        return this;
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
