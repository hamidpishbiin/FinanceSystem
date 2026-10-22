namespace FinanceSystem.Interface.Contracts.Payments.Models;

public class WalletPaymentModel : BasePaymentModel
{
    public required int FinanceAccountType { get; set; }
}
