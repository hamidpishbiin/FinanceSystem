namespace FinanceSystem.Interface.Contracts.Payments.Models;

public class CreatePaymentModel
{
    public int Purpose { get; set; }
    public int Channel { get; set; }
    public decimal Amount { get; set; }
    public long FinanceAccountId { get; set; }
    public int Direction { get; set; }
    public string ServiceId { get; set; }
    public string ReferenceId { get; set; }
    public int? PspCode { get; set; }
}
