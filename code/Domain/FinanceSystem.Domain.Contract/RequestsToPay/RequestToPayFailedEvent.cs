namespace FinanceSystem.Domain.Contract.RequestsToPay;

public record RequestToPayFailedEvent(long RequestToPayId)
    : RequestToPayEventBase(RequestToPayId);
