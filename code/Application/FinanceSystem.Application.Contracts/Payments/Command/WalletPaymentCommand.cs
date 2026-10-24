using FinanceSystem.Domain.FinanceAccounts.Enums;

namespace FinanceSystem.Application.Contracts.Payments.Command;

public class WalletPaymentCommand : PaymentCommand
{
    public required long BillId { get; set; }
    public required long FinanceAccountId { get; set; }
}
