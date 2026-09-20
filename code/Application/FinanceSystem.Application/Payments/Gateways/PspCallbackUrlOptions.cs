namespace FinanceSystem.Application.Payments.Gateways;

public class PspCallbackUrlOptions
{
    public const string SectionName = "PspCallbackUrls";
    public string TopUp { get; set; } = default!;
}
