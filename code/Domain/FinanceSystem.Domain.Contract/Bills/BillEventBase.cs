using Shared.Core.Events;

namespace FinanceSystem.Domain.Contract.Bills;

public abstract record BillEventBase(long BillId) : DomainEvent();
