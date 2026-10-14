namespace FinanceSystem.Domain.Contract.Payments;

public record PaymentReversedEvent(string ReferenceId) : PaymentEventBase(ReferenceId);
