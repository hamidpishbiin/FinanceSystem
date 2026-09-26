namespace FinanceSystem.Application.Contracts.Payments.Command;

public class CreatePaymentCommand : PaymentCommand
{
    public int Purpose { get; set; }
    public int Channel { get; set; }
    public decimal Amount { get; set; }
    public long SourceFinanceAccountId { get; set; }
    public long DestinationFinanceAccountId { get; set; }
    public string OriginServiceId { get; set; }
    public string ExternalReferenceId { get; set; }
    public string ExternalTag { get; set; }
    public long? RequestToPayId { get; set; }
}
