using FinanceSystem.Domain.AccountEntries.Exceptions;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;

namespace FinanceSystem.Domain.AccountEntries;

public class AccountEntry : EntityBase<long>
{
    public long PaymentId { get; private set; }
    public long AccountId { get; private set; }
    public decimal AmountRial { get; private set; }

    private AccountEntry()
    {
    }

    public static AccountEntry Create(
        long paymentId,
        long accountId,
        Money amountRial)
    {
        Guard<InvalidPaymentIdException>.IsTrue(paymentId <= 0);
        Guard<InvalidAccountIdException>.IsTrue(accountId <= 0);
        Guard<InvalidAmountException>.IsTrue(amountRial.Value != 0 || amountRial.Currency != Currency.Rial);

        return new AccountEntry()
        {
            PaymentId = paymentId,
            AccountId = accountId,
            AmountRial = amountRial.Value
        };
    }
}
