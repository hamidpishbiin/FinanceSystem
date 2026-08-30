using Shared.Domain;

namespace ProductManagement.Domain.Payments;

public interface IPaymentRepository : IRepository
{
    Task<Payment?> GetByIdAsync(long id);
    Task<Payment?> GetByIdempotencyKeyAsync(string idempotencyKey);
    Task AddAsync(Payment payment);
    void Update(Payment payment);
    void Delete(Payment payment);
}
