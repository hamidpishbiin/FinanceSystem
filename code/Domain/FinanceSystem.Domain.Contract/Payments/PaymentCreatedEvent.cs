namespace FinanceSystem.Domain.Contract.Payments;

public record PaymentCreatedEvent(string ReferenceId) : PaymentEventBase(ReferenceId);
