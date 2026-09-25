using Shared.Core.Events;

namespace FinanceSystem.Domain.Contract.Payments;

public abstract record PaymentEventBase(string IdempotencyKey, decimal Amount) : DomainEvent;
