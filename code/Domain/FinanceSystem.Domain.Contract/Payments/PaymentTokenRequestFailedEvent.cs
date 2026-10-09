namespace FinanceSystem.Domain.Contract.Payments;

public record PaymentTokenRequestFailedEvent(string ExternalReferenceId) : PaymentEventBase(ExternalReferenceId);
