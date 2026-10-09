namespace FinanceSystem.Domain.Contract.Payments;

public record PspTokenReceivedEvent(string ExternalReferenceId, string IpgUrl) : PaymentEventBase(ExternalReferenceId);
