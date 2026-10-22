using FinanceSystem.Domain.FinanceAccounts.Exceptions;
using FinanceSystem.Domain.LedgerEntries.Enums;
using FinanceSystem.Domain.LedgerEntries.Exceptions;
using FinanceSystem.Domain.Payments;
using FluentAssertions;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.Tests.Unit.LedgerEntryUnitTests;

public class LedgerEntryTests
{
    private readonly LedgerEntryBuilder _builder;

    public LedgerEntryTests()
    {
        _builder = new LedgerEntryBuilder();
    }

    [Fact]
    public void CreateInboundEntry_should_properly_create_inbound_ledgerEntry()
    {
        var ledgerEntry = _builder.BuildInbound();

        ledgerEntry.PaymentId.Should().Be(LedgerEntryBuilder.DefaultPaymentId);
        ledgerEntry.FinanceAccountId.Should().Be(LedgerEntryBuilder.DefaultFinanceAccountId);
        ledgerEntry.Amount.Should().Be(LedgerEntryBuilder.DefaultAmount);
        ledgerEntry.Direction.Should().Be(EntryDirection.Inbound);
    }

    [Fact]
    public void CreateOutboundEntry_should_properly_create_outbound_ledgerEntry()
    {
        var ledgerEntry = _builder.BuildOutbound();

        ledgerEntry.PaymentId.Should().Be(LedgerEntryBuilder.DefaultPaymentId);
        ledgerEntry.FinanceAccountId.Should().Be(LedgerEntryBuilder.DefaultFinanceAccountId);
        ledgerEntry.Amount.Should().Be(LedgerEntryBuilder.DefaultAmount);
        ledgerEntry.Direction.Should().Be(EntryDirection.Outbound);
    }

    [Fact]
    public void SignedAmount_should_be_positive_only_for_ingoing_entries()
    {
        var ledgerEntry = _builder.BuildInbound();

        ledgerEntry.SignedAmount.Should().Be(LedgerEntryBuilder.DefaultAmount);
    }

    [Fact]
    public void SignedAmount_should_be_negative_only_for_outgoing_entries()
    {
        var ledgerEntry = _builder.BuildOutbound();

        ledgerEntry.SignedAmount.Should().Be(-LedgerEntryBuilder.DefaultAmount);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_should_throw_when_PaymentId_is_not_positive(long paymentId)
    {
        Action act = () => _builder.WithPaymentId(paymentId).BuildInbound();

        act.Should().Throw<InvalidPaymentIdException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_should_throw_when_FinanceAccountId_is_not_positive(long financeAccountId)
    {
        Action act = () => _builder.WithFinanceAccountId(financeAccountId).BuildInbound();

        act.Should().Throw<InvalidFinanceAccountIdException>();
    }

    [Fact]
    public void Create_should_throw_when_Amount_is_null()
    {
        Action act = () => _builder.WithAmount(null!).BuildInbound();

        act.Should().Throw<NullEntryException>();
    }

    [Fact]
    public void Create_should_throw_when_Amount_is_zero()
    {
        Action act = () => _builder.WithAmount(new Money(0)).BuildInbound();

        act.Should().Throw<InvalidMoneyAmountException>();
    }
}
