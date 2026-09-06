using FinanceSystem.Domain.AccountEntries.Enums;
using FinanceSystem.Domain.AccountEntries.Exceptions;
using FinanceSystem.Domain.Accounts;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.AccountEntries;

public class AccountEntry : EntityBase<long>
{
    public long PaymentId { get; private set; }
    public long AccountId { get; private set; }
    public decimal AmountRial { get; private set; }
    public EntryDirection Direction { get; private set; }

    public Account? Account { get; private set; }
    public Payment? Payment { get; private set; }

    public decimal SignedAmountRial => Direction == EntryDirection.In ? AmountRial : -AmountRial;

    private AccountEntry()
    {
    }

    public static AccountEntry Create(
        long paymentId,
        long accountId,
        Money amountRial,
        EntryDirection direction)
    {
        Guard<InvalidPaymentIdException>.IsTrue(paymentId <= 0);
        Guard<InvalidAccountIdException>.IsTrue(accountId <= 0);
        Guard<NullEntryException>.AgainstNull(amountRial);
        Guard<InvalidAmountException>.IsTrue(amountRial.Value == 0);
        Guard<InvalidMoneyCurrencyException>.IsTrue(amountRial.Currency != Currency.Rial);
        Guard<InvalidEntryDirectionException>.IsFalse(Enum.IsDefined(direction));

        return new AccountEntry()
        {
            PaymentId = paymentId,
            AccountId = accountId,
            AmountRial = amountRial.Value,
            Direction = direction
        };
    }
}
