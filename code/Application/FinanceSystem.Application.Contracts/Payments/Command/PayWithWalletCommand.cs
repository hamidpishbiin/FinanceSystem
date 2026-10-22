using FinanceSystem.Domain.FinanceAccounts.Enums;

namespace FinanceSystem.Application.Contracts.Payments.Command;

public class PayWithWalletCommand : PaymentCommand
{
    public required long Token { get; set; }
    public required FinanceAccountType FinanceAccountType { get; set; }
}
