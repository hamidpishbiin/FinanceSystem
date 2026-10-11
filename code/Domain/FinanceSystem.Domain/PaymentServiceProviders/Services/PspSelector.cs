using Shared.Core;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Exceptions;

namespace FinanceSystem.Domain.PaymentServiceProviders.Services;

public class PspSelector(IPspRepository pspRepository) : IDomainService
{
    public async Task<PaymentServiceProvider> SelectAsync(PspCode? pspCode, CancellationToken cancellationToken)
    {
        var psp = pspCode.HasValue
            ? await pspRepository.GetByCodeAsync(pspCode.Value, cancellationToken)
            : await pspRepository.GetHighPriorityActiveAsync(cancellationToken);

        Guard<PspNotAvailableException>.IsTrue(psp is not { IsActive: true });

        return psp!;
    }
}
