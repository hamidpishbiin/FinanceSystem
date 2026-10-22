namespace FinanceSystem.Domain.AccountTypeRules;

public interface IAccountTypeRuleRepository : IRepository
{
    Task<List<AccountTypeRule>> GetTopUpRules(CancellationToken cancellationToken);
    Task<List<AccountTypeRule>> GetPurchaseRules(CancellationToken cancellationToken);
}
