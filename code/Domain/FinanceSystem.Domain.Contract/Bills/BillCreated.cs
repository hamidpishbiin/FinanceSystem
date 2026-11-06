using Shared.Core.Events;

namespace FinanceSystem.Domain.Contract.Bills;

public record BillCreated(long BillId) : BillEventBase(BillId);
