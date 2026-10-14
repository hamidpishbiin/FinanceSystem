namespace FinanceSystem.Domain.Contract.Payments;

public record PspTokenReceivedEvent(string ReferenceId, string IpgUrl) : PaymentEventBase(ReferenceId);
