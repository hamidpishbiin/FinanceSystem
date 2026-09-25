using FinanceSystem.Application.Payments.Gateways;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.PspPaymentDetails.Enums;
using Microsoft.Extensions.Options;

namespace FinanceSystem.Psp.BehPardakht;

internal class BehPardakhtGateway(
    IOptionsMonitor<PspOptions> pspOptions,
    IHttpClientFactory httpClientFactory) : IPspGateway
{
    public PspCode Code => PspCode.BehPardakht;

    private PspSettings Settings => pspOptions.CurrentValue.BehPardakht;

    private HttpClient CreateClient() => httpClientFactory.CreateClient(nameof(PspCode.BehPardakht));

    public Task<PspPaymentResponse> RequestPaymentTokenAsync(PspPaymentRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<PspVerifyResponse> VerifyPaymentAsync(PspVerifyRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private static PspFailureReason MapFailureReason(string resCode)
    {
        return resCode switch
        {
            "0" => PspFailureReason.None,
            "21" => PspFailureReason.InvalidTerminal,
            "61" => PspFailureReason.InvalidAmount,
            _ => PspFailureReason.Unknown
        };
    }
}
