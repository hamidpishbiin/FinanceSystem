using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Exceptions;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.PaymentServiceProviders;

public class PaymentServiceProvider : EntityBase<Guid>
{
    public PspCode Code { get; private set; }
    public string Name { get; private set; } = default!;
    public bool IsActive { get; private set; }
    public short Priority { get; private set; }

    private PaymentServiceProvider()
    {
    }

    public static async Task<PaymentServiceProvider> Create(
        Guid id,
        PspCode code,
        string name,
        bool isActive,
        short priority)
    {
        Guard<InvalidIdException>.IsTrue(id == Guid.Empty);
        Guard<InvalidPspCodeException>.IsFalse(Enum.IsDefined(code));
        Guard<InvalidPspNameException>.AgainstNullOrEmpty(name);
        Guard<InvalidPriorityException>.IsTrue(priority < 0);

        return new PaymentServiceProvider()
        {
            Id = id,
            Code = code,
            Name = name,
            IsActive = isActive,
            Priority = priority
        };
    }
}
