namespace FinanceSystem.Domain.Contract.PspPaymentDetails;

public record PspPaymentFailedEvent(long PspPaymentDetailId)
    : PspPaymentEventBase(PspPaymentDetailId);
