namespace FinanceSystem.Domain.Contract.Payments;

public record PaymentSucceeded(string ReferenceId) : PaymentEventBase(ReferenceId);
