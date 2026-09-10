using FinanceSystem.Domain.BankAccountDetails.Exceptions;
using FluentAssertions;

namespace FinanceSystem.Domain.Tests.Unit.BankAccountDetailUnitTests;

public class BankAccountDetailTests
{
    private readonly BankAccountDetailBuilder _builder;

    public BankAccountDetailTests()
    {
        _builder = new BankAccountDetailBuilder();
    }

    [Fact]
    public async Task Create_should_properly_create_bankAccountDetail()
    {
        var bad = await _builder.Build();

        bad.AccountId.Should().Be(BankAccountDetailBuilder.DefaultAccountId);
        bad.Iban.Should().Be(BankAccountDetailBuilder.DefaultIban);
        bad.MaskedPan.Should().Be(BankAccountDetailBuilder.DefaultMaskedPan);
        bad.BankName.Should().Be(BankAccountDetailBuilder.DefaultBankName);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Create_should_throw_when_accountId_is_not_positive(long accountId)
    {
        Func<Task> bad = () => _builder.WithAccountId(accountId).Build();

        await bad.Should().ThrowAsync<InvalidAccountIdException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Create_should_throw_when_iban_is_null_or_empty_or_whiteSpace(string? iban)
    {
        Func<Task> bad = () => _builder.WithIban(iban!).Build();

        await bad.Should().ThrowAsync<InvalidIbanException>();
    }

    [Fact]
    public async Task Create_should_allow_null_maskedPan_and_null_bankName()
    {
        var bad = await _builder.WithMaskedPan(null).WithBankName(null).Build();

        bad.MaskedPan.Should().BeNull();
        bad.BankName.Should().BeNull();
    }
}
