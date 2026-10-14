namespace FinanceSystem.Domain.Contract.Payments;

public record PaymentFailedEvent(string ReferenceId) : PaymentEventBase(ReferenceId);
