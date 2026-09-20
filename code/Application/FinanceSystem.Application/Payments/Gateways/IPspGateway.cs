using FinanceSystem.Domain.PaymentServiceProviders.Enums;

namespace FinanceSystem.Application.Payments.Gateways;

public interface IPspGateway
{
    PspCode Code { get; }
    Task<PspPaymentResponse> RequestPaymentTokenAsync(PspPaymentRequest request, CancellationToken cancellationToken);
    Task<PspVerifyResponse> VerifyPaymentAsync(PspVerifyRequest request, CancellationToken cancellationToken);
}
