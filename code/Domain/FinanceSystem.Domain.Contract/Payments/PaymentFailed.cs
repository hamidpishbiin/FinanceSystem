namespace FinanceSystem.Domain.Contract.Payments;

public record PaymentFailed(string ReferenceId) : PaymentEventBase(ReferenceId);
