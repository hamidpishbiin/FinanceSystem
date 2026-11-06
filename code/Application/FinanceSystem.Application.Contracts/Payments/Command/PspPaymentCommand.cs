using FinanceSystem.Domain.PaymentServiceProviders.Enums;

namespace FinanceSystem.Application.Contracts.Payments.Command;

public class PspPaymentCommand : PaymentCommand
{
    public required long BillId { get; set; }
    public PspCode PspCode { get; set; }
}
