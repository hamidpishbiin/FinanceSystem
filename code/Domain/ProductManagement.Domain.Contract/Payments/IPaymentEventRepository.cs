using Shared.Domain;

namespace ProductManagement.Domain.Contract.Payments;

public interface IPaymentEventRepository : IRepository
{
    Task Persist(PaymentEventModel happen);
}
