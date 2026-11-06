namespace FinanceSystem.Domain.Contract.Payments;

public record PaymentReversed(string ReferenceId) : PaymentEventBase(ReferenceId);
