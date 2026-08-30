using Shared.Core.Events;

namespace ProductManagement.Domain.Contract.Payments;

public class PaymentEventBase : DomainEvent
{
    public required string IdempotencyKey { get; set; }
    public required decimal AmountRial { get; set; }
}
