using FinanceSystem.Domain.BankAccountDetails.Exceptions;
using FinanceSystem.Domain.FinanceAccounts;

namespace FinanceSystem.Domain.BankAccountDetails;

public class BankAccountDetail : EntityBase<long>
{
    public long FinanceAccountId { get; private set; }
    public string Iban { get; private set; } = default!;
    public string? MaskedPan { get; private set; }
    public string? BankName { get; private set; }

    public FinanceAccount? FinanceAccount { get; private set; }

    private BankAccountDetail()
    {
    }

    public BankAccountDetail(
        long financeAccountId,
        string iban,
        string? maskedPan,
        string? bankName)
    {
        Guard<InvalidFinanceAccountIdException>.IsTrue(financeAccountId <= 0);
        Guard<InvalidIbanException>.AgainstNullOrEmpty(iban);

        FinanceAccountId = financeAccountId;
        Iban = iban;
        MaskedPan = maskedPan;
        BankName = bankName;
    }
}
