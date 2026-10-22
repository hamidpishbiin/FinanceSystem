using FinanceSystem.Domain.LedgerEntries;
using FinanceSystem.Domain.LedgerEntries.Enums;
using FinanceSystem.Domain.Payments;

namespace FinanceSystem.Domain.Tests.Unit.LedgerEntryUnitTests;

public class LedgerEntryBuilder
{
    public const long DefaultPaymentId = 12;
    public const long DefaultFinanceAccountId = 13;
    public const decimal DefaultAmount = 110;

    private long PaymentId { get; set; } = DefaultPaymentId;
    private long FinanceAccountId { get; set; } = DefaultFinanceAccountId;
    private Money Amount { get; set; } = new(DefaultAmount);

    public LedgerEntry BuildInbound()
    {
        return LedgerEntry.CreateInboundEntry(PaymentId, FinanceAccountId, Amount);
    }

    public LedgerEntry BuildOutbound()
    {
        return LedgerEntry.CreateOutboundEntry(PaymentId, FinanceAccountId, Amount);
    }

    public LedgerEntryBuilder WithPaymentId(long paymentId)
    {
        PaymentId = paymentId;
        return this;
    }

    public LedgerEntryBuilder WithFinanceAccountId(long financeAccountId)
    {
        FinanceAccountId = financeAccountId;
        return this;
    }

    public LedgerEntryBuilder WithAmount(Money amount)
    {
        Amount = amount;
        return this;
    }
}
