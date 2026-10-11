namespace FinanceSystem.Domain.Contract.Payments;

public record PaymentReversedEvent(string ExternalReferenceId) : PaymentEventBase(ExternalReferenceId);
