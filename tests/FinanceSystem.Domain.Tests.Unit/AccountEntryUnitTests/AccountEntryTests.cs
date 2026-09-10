using FinanceSystem.Domain.AccountEntries.Enums;
using FinanceSystem.Domain.AccountEntries.Exceptions;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using FluentAssertions;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.Tests.Unit.AccountEntryUnitTests;

public class AccountEntryTests
{
    private readonly AccountEntryBuilder _builder;

    public AccountEntryTests()
    {
        _builder = new AccountEntryBuilder();
    }

    [Fact]
    public async Task Create_should_properly_create_accountEntry()
    {
        var accountEntry = await _builder.Build();

        accountEntry.PaymentId.Should().Be(AccountEntryBuilder.DefaultPaymentId);
        accountEntry.AccountId.Should().Be(AccountEntryBuilder.DefaultAccountId);
        accountEntry.AmountRial.Should().Be(AccountEntryBuilder.DefaultAmountRial);
        accountEntry.Direction.Should().Be(AccountEntryBuilder.DefaultDirection);
    }

    [Theory]
    [InlineData(EntryDirection.In, 110)]
    [InlineData(EntryDirection.Out, -110)]
    public async Task SignedAmountRial_should_be_negative_only_for_outgoing_entries(
        EntryDirection direction,
        decimal expected)
    {
        var accountEntry = await _builder.WithDirection(direction).Build();

        accountEntry.SignedAmountRial.Should().Be(expected);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Create_should_throw_when_PaymentId_is_not_positive(long paymentId)
    {
        Func<Task> act = () => _builder.WithPaymentId(paymentId).Build();

        await act.Should().ThrowAsync<InvalidPaymentIdException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Create_should_throw_when_AccountId_is_not_positive(long accountId)
    {
        Func<Task> act = () => _builder.WithAccountId(accountId).Build();

        await act.Should().ThrowAsync<InvalidAccountIdException>();
    }

    [Fact]
    public async Task Create_should_throw_when_Amount_is_null()
    {
        Func<Task> act = () => _builder.WithAmount(null!).Build();

        await act.Should().ThrowAsync<NullEntryException>();
    }

    [Fact]
    public async Task Create_should_throw_when_Amount_is_zero()
    {
        Func<Task> act = () => _builder.WithAmount(new Money(0, Currency.Rial)).Build();

        await act.Should().ThrowAsync<InvalidAmountException>();
    }

    [Fact]
    public async Task Create_should_throw_when_Amount_is_not_in_Rial()
    {
        Func<Task> act = () => _builder.WithAmount(new Money(110, Currency.Toman)).Build();

        await act.Should().ThrowAsync<InvalidMoneyCurrencyException>();
    }

    [Theory]
    [InlineData((EntryDirection)0)]
    [InlineData((EntryDirection)99)]
    public async Task Create_should_throw_when_Direction_is_not_defined(EntryDirection direction)
    {
        Func<Task> act = () => _builder.WithDirection(direction).Build();

        await act.Should().ThrowAsync<InvalidEntryDirectionException>();
    }
}
