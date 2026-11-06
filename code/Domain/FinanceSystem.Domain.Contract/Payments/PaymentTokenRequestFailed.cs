namespace FinanceSystem.Domain.Contract.Payments;

public record PaymentTokenRequestFailed(string ReferenceId) : PaymentEventBase(ReferenceId);
