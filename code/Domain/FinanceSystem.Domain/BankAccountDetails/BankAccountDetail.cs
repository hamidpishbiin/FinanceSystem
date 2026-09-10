using FinanceSystem.Domain.BankAccountDetails.Exceptions;
using FinanceSystem.Domain.Accounts;

namespace FinanceSystem.Domain.BankAccountDetails;

public class BankAccountDetail : EntityBase<long>
{
    public long AccountId { get; private set; }
    public string Iban { get; private set; } = default!;
    public string? MaskedPan { get; private set; }
    public string? BankName { get; private set; }

    public Account? Account { get; private set; }

    private BankAccountDetail()
    {
    }

    public static async Task<BankAccountDetail> Create(
        long accountId,
        string iban,
        string? maskedPan,
        string? bankName)
    {
        Guard<InvalidAccountIdException>.IsTrue(accountId <= 0);
        Guard<InvalidIbanException>.AgainstNullOrEmpty(iban);

        return new BankAccountDetail()
        {
            AccountId = accountId,
            Iban = iban,
            MaskedPan = maskedPan,
            BankName = bankName
        };
    }
}
