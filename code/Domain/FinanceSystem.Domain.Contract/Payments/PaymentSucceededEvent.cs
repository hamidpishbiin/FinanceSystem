namespace FinanceSystem.Domain.Contract.Payments;

public record PaymentSucceededEvent(string ExternalReferenceId) : PaymentEventBase(ExternalReferenceId);
