using Shared.Core;
using Shared.EF;

namespace FinanceSystem.Saman;

public class SamanExchangeLogWriter(IDbContext context, IUnitOfWork unitOfWork)
{
    public async Task WriteAsync<TLog>(TLog exchangeLog, CancellationToken cancellationToken) where TLog : class
    {
        await context.DbSet<TLog>().AddAsync(exchangeLog, cancellationToken);

        await unitOfWork.SaveChangesAsync();
    }
}
