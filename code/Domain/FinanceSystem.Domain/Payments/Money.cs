using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.Payments;

public record Money
{
	public decimal Value { get; init; }

	public Money(decimal value)
	{
		Guard<InvalidMoneyAmountException>.IsTrue(value < 0);

        Value = value;
    }

    public override string ToString()
    {
        return $"{Value} Rial";
    }
}
