namespace FinanceSystem.Domain.Contract.Payments;

public record PspTokenReceivedEvent(string ExternalReferenceId, decimal Amount, string IpgUrl)
    : PaymentEventBase(ExternalReferenceId, Amount);
