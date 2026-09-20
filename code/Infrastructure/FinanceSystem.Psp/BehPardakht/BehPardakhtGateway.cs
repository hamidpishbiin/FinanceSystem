using FinanceSystem.Application.Payments.Gateways;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;

namespace FinanceSystem.Psp.BehPardakht;

public class BehPardakhtGateway : IPspGateway
{
    public PspCode Code => PspCode.BehPardakht;

    public Task<PspPaymentResponse> RequestPaymentTokenAsync(PspPaymentRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<PspVerifyResponse> VerifyPaymentAsync(PspVerifyRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
