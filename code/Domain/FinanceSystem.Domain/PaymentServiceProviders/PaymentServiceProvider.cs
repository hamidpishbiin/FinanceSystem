namespace FinanceSystem.Domain.PaymentServiceProviders;

public class PaymentServiceProvider : EntityBase<Guid>
{
    public PsPCode Code { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public short Priority { get; set; }
    public string MerchantId { get; set; }
    public string? TerminalId { get; set; }
    public string CredentialsRef { get; set; }
    public string BaseUrl { get; set; }
    public string CallbackUrl { get; set; }
}
