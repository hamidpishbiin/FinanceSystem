using FinanceSystem.Domain.Payments.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;

namespace FinanceSystem.Application.Payments.Gateways;

public interface IPspGateway
{
    PspCode Code { get; }
    Task<PspTokenResponse> RequestTokenAsync(PspTokenRequest request, CancellationToken cancellationToken);
    Task<PaymentVerificationResponse> VerifyPaymentAsync(PaymentVerificationRequest request, CancellationToken cancellationToken);
    PspCallback ParseCallback(IReadOnlyDictionary<string, string> fields);
}
