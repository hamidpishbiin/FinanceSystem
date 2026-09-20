namespace FinanceSystem.Domain.PspPaymentDetails;

public interface IPspPaymentDetailRepository : IRepository
{
    Task AddAsync(PspPaymentDetail pspPaymentDetail, CancellationToken cancellationToken);
    Task<PspPaymentDetail?> GetByReferenceNumberAsync(long referenceNumber, CancellationToken cancellationToken);
}
