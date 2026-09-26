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
    public void Create_should_properly_create_bankAccountDetail()
    {
        var bad = _builder.Build();

        bad.FinanceAccountId.Should().Be(BankAccountDetailBuilder.DefaultFinanceAccountId);
        bad.Iban.Should().Be(BankAccountDetailBuilder.DefaultIban);
        bad.MaskedPan.Should().Be(BankAccountDetailBuilder.DefaultMaskedPan);
        bad.BankName.Should().Be(BankAccountDetailBuilder.DefaultBankName);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_should_throw_when_financeAccountId_is_not_positive(long financeAccountId)
    {
        Action bad = () => _builder.WithFinanceAccountId(financeAccountId).Build();

        bad.Should().Throw<InvalidFinanceAccountIdException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_should_throw_when_iban_is_null_or_empty_or_whiteSpace(string? iban)
    {
        Action bad = () => _builder.WithIban(iban!).Build();

        bad.Should().Throw<InvalidIbanException>();
    }

    [Fact]
    public void Create_should_allow_null_maskedPan_and_null_bankName()
    {
        var bad = _builder.WithMaskedPan(null).WithBankName(null).Build();

        bad.MaskedPan.Should().BeNull();
        bad.BankName.Should().BeNull();
    }
}
