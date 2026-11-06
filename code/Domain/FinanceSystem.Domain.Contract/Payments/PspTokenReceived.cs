namespace FinanceSystem.Domain.Contract.Payments;

public record PspTokenReceived(string ReferenceId) : PaymentEventBase(ReferenceId);
