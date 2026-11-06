namespace FinanceSystem.Domain.Contract.Payments;

public record PaymentCreated(string ReferenceId) : PaymentEventBase(ReferenceId);
