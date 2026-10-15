using FinanceSystem.Domain.Bills.Exceptions;
using FinanceSystem.Domain.Contract.Bills;
using FinanceSystem.Domain.Payments;
using Shared.Domain.Exceptions;
using InvalidServiceIdException = FinanceSystem.Domain.Payments.Exceptions.InvalidServiceIdException;

namespace FinanceSystem.Domain.Bills;

public class Bill : EntityBase<long>, IAggregateRoot
{
    public IEventPublisher Publisher { get; set; }
    public long ServiceId { get; private set; }
    public long ReferenceId { get; private set; }
    public decimal Amount { get; private set; }

    private Bill(){}

    public Bill(
        long serviceId,
        long referenceId,
        Money amount)
    {
        Guard<InvalidReferenceIdException>.IsTrue(referenceId <= 0);
        Guard<Exceptions.InvalidServiceIdException>.IsTrue(serviceId <= 0);
        Guard<NullEntryException>.AgainstNull(amount);
        Guard<InvalidMoneyAmountException>.IsTrue(amount.Value == 0);

        ServiceId = serviceId;
        ReferenceId = referenceId;
        Amount = amount.Value;
    }
}
