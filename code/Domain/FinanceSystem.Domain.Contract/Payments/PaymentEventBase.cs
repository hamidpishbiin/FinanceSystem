using Shared.Core.Events;

namespace FinanceSystem.Domain.Contract.Payments;

public abstract record PaymentEventBase(string ExternalReferenceId, decimal Amount) : DomainEvent;
