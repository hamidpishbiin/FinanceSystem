using FinanceSystem.Domain.PspPaymentDetails;

namespace FinanceSystem.Persistance.Repositories;

public class PspPaymentDetailRepository(IDbContext context) : IPspPaymentDetailRepository
{
    private DbSet<PspPaymentDetail> DbSet => context.DbSet<PspPaymentDetail>();

    public async Task AddAsync(PspPaymentDetail pspPaymentDetail, CancellationToken cancellationToken)
    {
        await DbSet.AddAsync(pspPaymentDetail, cancellationToken);
    }

    public async Task<PspPaymentDetail?> GetByReferenceNumberAsync(long referenceNumber, CancellationToken cancellationToken)
    {
        return await DbSet.FirstOrDefaultAsync(d => d.ReferenceNumber == referenceNumber, cancellationToken);
    }
}
