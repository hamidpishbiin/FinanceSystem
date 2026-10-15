using Shared.Core.Events;

namespace FinanceSystem.Domain.Contract.Bills;

public record BillCreatedEvent(long BillId) : BillEventBase(BillId);
