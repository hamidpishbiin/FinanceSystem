using Shared.Core.Events;

namespace FinanceSystem.Domain.Contract.PspPaymentDetails;

public abstract record PspPaymentEventBase(long PspPaymentDetailId) : DomainEvent;
