using FinanceSystem.Domain.Payments.Enums;
using FinanceSystem.Domain.Payments.Exceptions;

namespace FinanceSystem.Domain.Payments;

public record Money
{
	public decimal Value { get; init; }
	public Currency Currency { get; init; }

	public Money(decimal value, Currency currency)
	{
		Guard<NegativeMoneyAmountException>.IsTrue(value < 0);
        Guard<InvalidCurrencyException>.IsFalse(Enum.IsDefined(currency));

        Value = value;
        Currency = currency;
    }

    public override string ToString()
    {
        return $"{Value} {Currency}";
    }
}
