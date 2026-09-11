using FinanceSystem.Domain.PaymentServiceProviders;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;

namespace FinanceSystem.Domain.Tests.Unit.PaymentServiceProviderUnitTests;

public class PaymentServiceProviderBuilder
{
    public const string DefaultIdString = "9f1d2c3b-4a5e-6f70-8192-a3b4c5d6e7f8";
    public const PsPCode DefaultCode = PsPCode.Saman;
    public const string DefaultName = "Saman Bank Gateway";
    public const bool DefaultIsActive = true;
    public const short DefaultPriority = 10;
    public const string DefaultMerchantId = "merchant-55012";
    public const string DefaultTerminalId = "terminal-88";
    public const string DefaultCredentialsRef = "vault://psp/saman";
    public const string DefaultBaseUrl = "https://sep.shaparak.ir";
    public const string DefaultCallbackUrl = "https://finance.local/psp/callback";

    public static readonly Guid DefaultId = Guid.Parse(DefaultIdString);

    private Guid Id { get; set; } = DefaultId;
    private PsPCode Code { get; set; } = DefaultCode;
    private string Name { get; set; } = DefaultName;
    private bool IsActive { get; set; } = DefaultIsActive;
    private short Priority { get; set; } = DefaultPriority;
    private string MerchantId { get; set; } = DefaultMerchantId;
    private string? TerminalId { get; set; } = DefaultTerminalId;
    private string CredentialsRef { get; set; } = DefaultCredentialsRef;
    private string BaseUrl { get; set; } = DefaultBaseUrl;
    private string CallbackUrl { get; set; } = DefaultCallbackUrl;

    public async Task<PaymentServiceProvider> Build()
    {
        return await PaymentServiceProvider.Create(
            Id,
            Code,
            Name,
            IsActive,
            Priority,
            MerchantId,
            TerminalId,
            CredentialsRef,
            BaseUrl,
            CallbackUrl);
    }

    public PaymentServiceProviderBuilder WithId(Guid id)
    {
        Id = id;
        return this;
    }

    public PaymentServiceProviderBuilder WithCode(PsPCode code)
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

    public PaymentServiceProviderBuilder WithMerchantId(string? merchantId)
    {
        MerchantId = merchantId!;
        return this;
    }

    public PaymentServiceProviderBuilder WithTerminalId(string? terminalId)
    {
        TerminalId = terminalId;
        return this;
    }

    public PaymentServiceProviderBuilder WithCredentialsRef(string? credentialsRef)
    {
        CredentialsRef = credentialsRef!;
        return this;
    }

    public PaymentServiceProviderBuilder WithBaseUrl(string? baseUrl)
    {
        BaseUrl = baseUrl!;
        return this;
    }

    public PaymentServiceProviderBuilder WithCallbackUrl(string? callbackUrl)
    {
        CallbackUrl = callbackUrl!;
        return this;
    }
}
