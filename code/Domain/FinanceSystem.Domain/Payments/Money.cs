using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.Payments;

public class Money : ValueObjectBase<Money>
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

    public override bool SameValueAs(Money valueObject)
    {
        return Value.Equals(valueObject.Value);
    }

    public override int HashCode()
    {
        return Value.GetHashCode();
    }
}
