namespace FinanceSystem.Domain.Contract.Payments;

public record PaymentCreatedEvent(string ExternalReferenceId) : PaymentEventBase(ExternalReferenceId);
