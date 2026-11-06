namespace FinanceSystem.Domain.Contract.Payments;

public record PspTokenReceivedEvent(string ReferenceId) : PaymentEventBase(ReferenceId);
