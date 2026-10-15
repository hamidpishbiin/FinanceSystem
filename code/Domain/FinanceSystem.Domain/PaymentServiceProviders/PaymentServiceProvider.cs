using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Exceptions;

namespace FinanceSystem.Domain.PaymentServiceProviders;

public class PaymentServiceProvider : EntityBase<long>
{
    public PspCode Code { get; private set; }
    public string Name { get; private set; } = default!;
    public bool IsActive { get; private set; }
    public short Priority { get; private set; }

    private PaymentServiceProvider()
    {
    }

    public PaymentServiceProvider(
        PspCode code,
        string name,
        bool isActive,
        short priority)
    {
        Guard<InvalidPspCodeException>.IsFalse(Enum.IsDefined(code));
        Guard<InvalidPspNameException>.AgainstNullOrEmpty(name);
        Guard<InvalidPriorityException>.IsTrue(priority < 0);

        Code = code;
        Name = name;
        IsActive = isActive;
        Priority = priority;
    }
}
