using FinanceSystem.Domain.AccountTypeRules;
using FinanceSystem.Domain.FinanceAccounts.Enums;
using FinanceSystem.Domain.FinanceAccounts.Exceptions;
using Shared.Core;

namespace FinanceSystem.Domain.FinanceAccounts.DomainServices;

public class FinanceAccountEligibilityService(
    IFinanceAccountRepository repository,
    IAccountTypeRuleRepository accountTypeRuleRepository) : IDomainService
{
    public async Task<FinanceAccount> GetEligibleTopUpAccountAsync(long userId, CancellationToken cancellationToken)
    {
        var accounts = await repository.GetByUserIdAsync(userId, cancellationToken);

        Guard<FinanceAccountNotFoundException>.IsTrue(accounts.Count == 0);

        var rules = await accountTypeRuleRepository.GetTopUpRules(cancellationToken);

        var validAccounts = accounts.Where(a => a.IsTopUpAllowed(rules)).ToList();

        Guard<ValidTopUpAccountNotFoundException>.IsTrue(validAccounts.Count == 0);

        Guard<MoreThanOneValidTopUpAccountFoundException>.IsTrue(validAccounts.Count > 1);

        return validAccounts.First();
    }

    public async Task<FinanceAccount> GetEligibleWalletPurchaseAccountAsync(long userId, FinanceAccountType accountType, CancellationToken cancellationToken)
    {
        var accounts = await repository.GetByUserIdAsync(userId, cancellationToken);

        Guard<FinanceAccountNotFoundException>.IsTrue(accounts.Count == 0);

        var rules = await accountTypeRuleRepository.GetPurchaseRules(cancellationToken);

        var validAccounts = accounts
            .Where(a => a.Type == accountType)
            .Where(a => a.IsAllowedToPurchase(rules))
            .ToList();

        Guard<ValidWalletPurchaseAccountNotFoundException>.IsTrue(validAccounts.Count == 0);

        Guard<MoreThanOneValidWalletPurchaseAccountFoundException>.IsTrue(validAccounts.Count > 1);

        return validAccounts.First();
    }

    public async Task<FinanceAccount> GetBillTargetAccountAsync(long userId, CancellationToken cancellationToken)
    {
        var account = await repository.GetTargetAccount(userId, FinanceAccountType.UserWallet, [FinanceAccountState.Active], cancellationToken);

        Guard<FinanceAccountNotFoundException>.AgainstNull(account);

        return account!;
    }
}
