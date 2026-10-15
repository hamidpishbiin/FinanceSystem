using FinanceSystem.Domain.FinanceAccounts.Exceptions;
using Shared.Core;

namespace FinanceSystem.Domain.FinanceAccounts.DomainServices;

public class FinanceAccountEligibilityService(IFinanceAccountRepository repository) : IDomainService
{
    public async Task<FinanceAccount?> GetTopUpTargetAccountAsync(long userId, CancellationToken cancellationToken)
    {
        var account =  await repository.GetTopUpTargetAccountAsync(userId, cancellationToken);

        Guard<FinanceAccountNotFoundException>.AgainstNull(account);

        return account;
    }
}
