namespace FinanceSystem.Domain.Contract.Payments;

public record PaymentCreatedEvent(string ExternalReferenceId, decimal Amount)
    : PaymentEventBase(ExternalReferenceId, Amount);
