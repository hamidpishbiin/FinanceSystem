using Shared.Domain;

namespace FinanceSystem.Domain.Contract.Payments;

public interface IPaymentEventRepository : IRepository
{
    Task Persist(PaymentEventModel happen);
}
