using ProductManagement.Domain.Payments.Enums;
using ProductManagement.Domain.Payments.Exceptions;

namespace ProductManagement.Domain.Payments;

public record Money
{
	public decimal Amount { get; init; }
	public Currency Currency { get; init; }

	public Money(decimal amount, Currency currency = Currency.Rial)
	{
		Guard<NegativeMoneyAmountException>.IsTrue(amount < 0);
        Guard<InvalidCurrencyException>.IsFalse(Enum.IsDefined(currency));

        Amount = amount;
        Currency = currency;
    }

    public override string ToString()
    {
        return $"{Amount} {Currency}";
    }
}
