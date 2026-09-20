using FinanceSystem.Domain.PaymentServiceProviders.Enums;

namespace FinanceSystem.Application.Payments.Gateways;

public interface IPspGatewayFactory
{
    IPspGateway Get(PspCode pspCode);
}
