namespace FinanceSystem.Domain.Contract.PspPaymentDetails;

public record PspPaymentTokenReceivedEvent(long PspPaymentDetailId, string IpgUrl)
    : PspPaymentEventBase(PspPaymentDetailId);
