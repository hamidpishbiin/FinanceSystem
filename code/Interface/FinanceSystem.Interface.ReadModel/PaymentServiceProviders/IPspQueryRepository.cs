using FinanceSystem.Domain.PaymentServiceProviders;
using Shared.Domain;

namespace FinanceSystem.Interface.ReadModel.PaymentServiceProviders;

public interface IPspQueryRepository : IRepository
{
    Task<List<PaymentServiceProvider>> GetActivePspsAsync(CancellationToken cancellationToken);
}
