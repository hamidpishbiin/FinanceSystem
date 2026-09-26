using FinanceSystem.Domain.AccountEntries.Enums;
using FinanceSystem.Domain.AccountEntries.Exceptions;
using FinanceSystem.Domain.FinanceAccounts;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.AccountEntries;

public class AccountEntry : EntityBase<long>
{
    public long PaymentId { get; private set; }
    public long FinanceAccountId { get; private set; }
    public decimal Amount { get; private set; }
    public EntryDirection Direction { get; private set; }

    public FinanceAccount? FinanceAccount { get; private set; }
    public Payment? Payment { get; private set; }

    public decimal SignedAmount => Direction == EntryDirection.In ? Amount : -Amount;

    private AccountEntry()
    {
    }

    public AccountEntry(
        long paymentId,
        long financeAccountId,
        Money amount,
        EntryDirection direction)
    {
        Guard<InvalidPaymentIdException>.IsTrue(paymentId <= 0);
        Guard<InvalidFinanceAccountIdException>.IsTrue(financeAccountId <= 0);
        Guard<NullEntryException>.AgainstNull(amount);
        Guard<InvalidAmountException>.IsTrue(amount.Value == 0);
        Guard<InvalidEntryDirectionException>.IsFalse(Enum.IsDefined(direction));

        PaymentId = paymentId;
        FinanceAccountId = financeAccountId;
        Amount = amount.Value;
        Direction = direction;
    }
}
