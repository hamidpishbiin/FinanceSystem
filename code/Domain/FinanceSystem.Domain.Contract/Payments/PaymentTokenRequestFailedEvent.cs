namespace FinanceSystem.Domain.Contract.Payments;

public record PaymentTokenRequestFailedEvent(string ExternalReferenceId, decimal Amount)
    : PaymentEventBase(ExternalReferenceId, Amount);
