using Shared.Core.Events;

namespace FinanceSystem.Domain.Contract.RequestsToPay;

public abstract record RequestToPayEventBase(long RequestToPayId) : DomainEvent;
