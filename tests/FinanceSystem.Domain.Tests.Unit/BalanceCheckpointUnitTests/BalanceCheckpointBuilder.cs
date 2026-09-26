using FinanceSystem.Domain.BalanceCheckpoints;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;

namespace FinanceSystem.Domain.Tests.Unit.BalanceCheckpointUnitTests;

public class BalanceCheckpointBuilder
{
    public const long DefaultFinanceAccountId = 1234;
    public const long DefaultUpToEntryId = 5678;
    public const decimal DefaultBalance = 9086131;

    private long FinanceAccountId { get; set; } = DefaultFinanceAccountId;
    private long UpToEntryId { get; set; } = DefaultUpToEntryId;
    private Money BalanceAmount { get; set; } = new Money(DefaultBalance);

    public BalanceCheckpoint Build()
    {
        return new BalanceCheckpoint(FinanceAccountId, UpToEntryId, BalanceAmount);
    }

    public BalanceCheckpointBuilder WithFinanceAccountId(long financeAccountId)
    {
        FinanceAccountId = financeAccountId;
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
