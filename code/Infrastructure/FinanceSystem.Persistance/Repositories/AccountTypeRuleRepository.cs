using FinanceSystem.Domain.AccountTypeRules;
using FinanceSystem.Domain.Payments.Enums;

namespace FinanceSystem.Persistance.Repositories;

internal class AccountTypeRuleRepository : IAccountTypeRuleRepository
{
    private readonly DbSet<AccountTypeRule> _dbSet;

    public AccountTypeRuleRepository(IDbContext context)
    {
        _dbSet = context.DbSet<AccountTypeRule>();
    }

    public async Task<List<AccountTypeRule>> GetTopUpRules(CancellationToken cancellationToken)
    {
        return await GetByPurposeAsync(PaymentPurpose.TopUp, cancellationToken);
    }

    public async Task<List<AccountTypeRule>> GetPurchaseRules(CancellationToken cancellationToken)
    {
        return await GetByPurposeAsync(PaymentPurpose.Purchase, cancellationToken);
    }

    private async Task<List<AccountTypeRule>> GetByPurposeAsync(PaymentPurpose purpose, CancellationToken cancellationToken)
    {
        return await _dbSet.Where(atr => atr.Purpose == purpose).ToListAsync(cancellationToken);
    }
}
