namespace FinanceSystem.Domain.Contract.RequestsToPay;

public record RequestToPayTokenReceivedEvent(long RequestToPayId, string IpgUrl)
    : RequestToPayEventBase(RequestToPayId);
