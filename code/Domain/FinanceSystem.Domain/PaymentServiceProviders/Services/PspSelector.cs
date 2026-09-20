using Shared.Core;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;

namespace FinanceSystem.Domain.PaymentServiceProviders.Services;

public class PspSelector(IPspRepository pspRepository) : IDomainService
{
    public async Task<PaymentServiceProvider> SelectAsync(PspCode? pspCode, CancellationToken cancellationToken)
    {
        if (pspCode.HasValue)
        {
            return await pspRepository.GetByCodeAsync(pspCode.Value, cancellationToken) ?? throw new InvalidOperationException("PSP not found");
        }

        return await pspRepository.GetHighPriorityActiveAsync(cancellationToken) ?? throw new InvalidOperationException($"Psp not found for code {pspCode}");
    }
}
