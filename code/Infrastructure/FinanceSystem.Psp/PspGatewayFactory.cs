using FinanceSystem.Application.Payments.Gateways;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;

namespace FinanceSystem.Psp;

internal class PspGatewayFactory(IEnumerable<IPspGateway> gateways) : IPspGatewayFactory
{
    public IPspGateway Get(PspCode pspCode)
    {
        return gateways.FirstOrDefault(gw => gw.Code == pspCode) ??
               throw new InvalidOperationException($"No PSP found for code {pspCode}");
    }
}
