using Shared.Core;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Exceptions;

namespace FinanceSystem.Domain.PaymentServiceProviders.Services;

public class PspSelector(IPspRepository pspRepository) : IDomainService
{
    public async Task<PaymentServiceProvider> SelectAsync(int pspCode, CancellationToken cancellationToken)
    {
        var pspEnum = (PspCode)pspCode;

        var psp = Enum.IsDefined(pspEnum)
            ? await pspRepository.GetByCodeAsync(pspEnum, cancellationToken)
            : await pspRepository.GetHighPriorityActiveAsync(cancellationToken);

        Guard<PspNotAvailableException>.IsTrue(psp is not { IsActive: true });

        return psp!;
    }
}
