namespace FinanceSystem.Interface.Contracts.Payments.Models;

public class BehPardakhtCallbackModel
{
    public string? ResCode { get; set; }
    public string? SaleOrderId { get; set; }
    public string? SaleReferenceId { get; set; }
    public string? CardHolderPan { get; set; }
    public string? RefId { get; set; }
    public uint? FinalAmount { get; set; }
}
