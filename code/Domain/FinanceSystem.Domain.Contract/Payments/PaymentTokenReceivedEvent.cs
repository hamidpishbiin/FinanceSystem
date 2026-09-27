namespace FinanceSystem.Domain.Contract.Payments;

public record PaymentTokenReceivedEvent(string ExternalReferenceId, decimal Amount, string IpgUrl)
    : PaymentEventBase(ExternalReferenceId, Amount);
