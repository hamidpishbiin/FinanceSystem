using Shared.Core.Events;

namespace FinanceSystem.Domain.Contract.Payments;

public class PaymentEventBase : DomainEvent
{
    public required string IdempotencyKey { get; set; }
    public required decimal AmountRial { get; set; }
}
