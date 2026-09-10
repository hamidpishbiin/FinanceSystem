using FinanceSystem.Domain.BankAccountDetails;

namespace FinanceSystem.Domain.Tests.Unit.BankAccountDetailUnitTests;

public class BankAccountDetailBuilder
{
    public const long DefaultAccountId = 1276;
    public const string DefaultIban = "kqskjdjhflaskdjfhlskadjfh";
    public const string DefaultMaskedPan = "6219********1471";
    public const string DefaultBankName = "Mellat";

    private long AccountId { get; set; } = DefaultAccountId;
    private string Iban { get; set; } = DefaultIban;
    private string? MaskedPan { get; set; } = DefaultMaskedPan;
    private string? BankName { get; set; } = DefaultBankName;

    public async Task<BankAccountDetail> Build()
    {
        return await BankAccountDetail.Create(AccountId, Iban, MaskedPan, BankName);
    }

    public BankAccountDetailBuilder WithAccountId(long accountId)
    {
        AccountId = accountId;
        return this;
    }

    public BankAccountDetailBuilder WithIban(string iban)
    {
        Iban = iban;
        return this;
    }

    public BankAccountDetailBuilder WithMaskedPan(string? maskedPan)
    {
        MaskedPan = maskedPan;
        return this;
    }

    public BankAccountDetailBuilder WithBankName(string? bankName)
    {
        BankName = bankName;
        return this;
    }
}
