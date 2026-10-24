namespace FinanceSystem.Presentation;

public class PaymentResultPageConfig
{
    public const string SectionName = "PaymentResultPageConfig";

    public required string SuccessUrl { get; set; }
    public required string FailureUrl { get; set; }
}
