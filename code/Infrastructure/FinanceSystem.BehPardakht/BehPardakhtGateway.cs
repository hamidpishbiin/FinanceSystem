using FinanceSystem.Application.Payments.Gateways;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.Payments.Enums;
using Microsoft.Extensions.Options;

namespace FinanceSystem.BehPardakht;

internal class BehPardakhtGateway(
    IOptionsMonitor<PspOptions> pspOptions,
    IHttpClientFactory httpClientFactory) : IPspGateway
{
    public PspCode Code => PspCode.BehPardakht;

    private PspSettings Settings => pspOptions.CurrentValue.BehPardakht;

    private HttpClient CreateClient() => httpClientFactory.CreateClient(nameof(PspCode.BehPardakht));

    public Task<PspTokenResponse> RequestPaymentTokenAsync(PspTokenRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<PaymentVerificationResponse> VerifyPaymentAsync(PaymentVerificationRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<PaymentVerificationResponse> ReversePaymentAsync(PaymentVerificationRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public PspCallback ParseCallback(IReadOnlyDictionary<string, string> fields)
    {
        throw new NotImplementedException();
    }
}
