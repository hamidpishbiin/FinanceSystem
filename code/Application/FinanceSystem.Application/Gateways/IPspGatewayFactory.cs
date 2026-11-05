using FinanceSystem.Domain.PaymentServiceProviders.Enums;

namespace FinanceSystem.Application.Gateways;

public interface IPspGatewayFactory
{
    IPspGateway Get(PspCode pspCode);
}
