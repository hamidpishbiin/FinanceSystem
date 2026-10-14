namespace FinanceSystem.Domain.Contract.Payments;

public record PaymentSucceededEvent(string ReferenceId) : PaymentEventBase(ReferenceId);
