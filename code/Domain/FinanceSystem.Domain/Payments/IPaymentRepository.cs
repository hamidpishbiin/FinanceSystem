
namespace FinanceSystem.Domain.Payments;

public interface IPaymentRepository : IRepository
{
    Task<Payment?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task AddAsync(Payment payment);
    Task<Payment?> GetByReferenceNumberAsync(string referenceNumber, CancellationToken cancellationToken);
}
