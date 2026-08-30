namespace ProductManagement.Interface.Contracts.Payments.Models;

public class CreatePaymentModel
{
    public string IdempotencyKey { get; set; }
    public int Purpose { get; set; }
    public int Channel { get; set; }
    public decimal AmountRial { get; set; }
    public long SourceAccountId { get; set; }
    public long DestinationAccountId { get; set; }
    public string OriginServiceId { get; set; }
    public string ExternalReferenceId { get; set; }
    public string ExternalTag { get; set; }
    public long? BankPaymentDetailId { get; set; }
}
