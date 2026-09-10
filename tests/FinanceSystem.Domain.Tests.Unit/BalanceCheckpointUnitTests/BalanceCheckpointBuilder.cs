using FinanceSystem.Domain.BalanceCheckpoints;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;

namespace FinanceSystem.Domain.Tests.Unit.BalanceCheckpointUnitTests;

public class BalanceCheckpointBuilder
{
    public const long DefaultAccountId = 1234;
    public const long DefaultUpToEntryId = 5678;
    public const decimal DefaultBalanceRial = 9086131;

    private long AccountId { get; set; } = DefaultAccountId;
    private long UpToEntryId { get; set; } = DefaultUpToEntryId;
    private Money BalanceAmount { get; set; } = new Money(DefaultBalanceRial, Currency.Rial);

    public async Task<BalanceCheckpoint> Build()
    {
        return await BalanceCheckpoint.Create(AccountId, UpToEntryId, BalanceAmount);
    }

    public BalanceCheckpointBuilder WithAccountId(long accountId)
    {
        AccountId = accountId;
        return this;
    }

    public BalanceCheckpointBuilder WithUpToEntryId(long upToEntryId)
    {
        UpToEntryId = upToEntryId;
        return this;
    }

    public BalanceCheckpointBuilder WithBalanceAmount(Money balanceAmount)
    {
        BalanceAmount = balanceAmount;
        return this;
    }
}
