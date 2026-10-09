namespace FinanceSystem.Domain.Contract.Payments;

public record PaymentFailedEvent(string ExternalReferenceId) : PaymentEventBase(ExternalReferenceId);
