using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Exceptions;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.PaymentServiceProviders;

public class PaymentServiceProvider : EntityBase<Guid>
{
    public PsPCode Code { get; private set; }
    public string Name { get; private set; } = default!;
    public bool IsActive { get; private set; }
    public short Priority { get; private set; }
    public string MerchantId { get; private set; } = default!;
    public string? TerminalId { get; private set; }
    public string CredentialsRef { get; private set; } = default!;
    public string BaseUrl { get; private set; } = default!;
    public string CallbackUrl { get; private set; } = default!;

    private PaymentServiceProvider()
    {
    }

    public static async Task<PaymentServiceProvider> Create(
        Guid id,
        PsPCode code,
        string name,
        bool isActive,
        short priority,
        string merchantId,
        string? terminalId,
        string credentialsRef,
        string baseUrl,
        string callbackUrl)
    {
        Guard<InvalidIdException>.IsTrue(id == Guid.Empty);
        Guard<InvalidPspCodeException>.IsFalse(Enum.IsDefined(code));
        Guard<InvalidPspNameException>.AgainstNullOrEmpty(name);
        Guard<InvalidPriorityException>.IsTrue(priority < 0);
        Guard<InvalidMerchantIdException>.AgainstNullOrEmpty(merchantId);
        Guard<InvalidCredentialsRefException>.AgainstNullOrEmpty(credentialsRef);
        Guard<InvalidBaseUrlException>.AgainstNullOrEmpty(baseUrl);
        Guard<InvalidCallbackUrlException>.AgainstNullOrEmpty(callbackUrl);

        return new PaymentServiceProvider()
        {
            Id = id,
            Code = code,
            Name = name,
            IsActive = isActive,
            Priority = priority,
            MerchantId = merchantId,
            TerminalId = terminalId,
            CredentialsRef = credentialsRef,
            BaseUrl = baseUrl,
            CallbackUrl = callbackUrl
        };
    }
}
