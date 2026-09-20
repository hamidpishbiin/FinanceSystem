using FinanceSystem.Application.Payments.Gateways;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;

namespace FinanceSystem.Psp.Saman;

public class SamanGateway : IPspGateway
{
    public PspCode Code => PspCode.Saman;

    public Task<PspPaymentResponse> RequestPaymentTokenAsync(PspPaymentRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<PspVerifyResponse> VerifyPaymentAsync(PspVerifyRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
