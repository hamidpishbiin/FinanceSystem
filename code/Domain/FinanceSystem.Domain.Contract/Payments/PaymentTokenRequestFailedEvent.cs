namespace FinanceSystem.Domain.Contract.Payments;

public record PaymentTokenRequestFailedEvent(string ReferenceId) : PaymentEventBase(ReferenceId);
