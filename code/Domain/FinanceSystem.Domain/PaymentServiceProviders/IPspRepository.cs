using FinanceSystem.Domain.PaymentServiceProviders.Enums;

namespace FinanceSystem.Domain.PaymentServiceProviders;

public interface IPspRepository : IRepository
{
    Task<IReadOnlyCollection<PaymentServiceProvider>> GetAllActiveAsync(CancellationToken cancellationToken);
    Task<PaymentServiceProvider?> GetByCodeAsync(PspCode code, CancellationToken cancellationToken);

    Task<PaymentServiceProvider?> GetHighPriorityActiveAsync(CancellationToken cancellationToken);
}
