using Shared.Core;
using Shared.EF;

namespace FinanceSystem.Saman;

public class SamanTokenExchangeLogWriter(IDbContext context, IUnitOfWork unitOfWork)
{
    public async Task WriteAsync(SamanTokenExchangeLogModel exchangeLog, CancellationToken cancellationToken)
    {
        await context.DbSet<SamanTokenExchangeLogModel>().AddAsync(exchangeLog, cancellationToken);

        await unitOfWork.SaveChangesAsync();
    }
}
