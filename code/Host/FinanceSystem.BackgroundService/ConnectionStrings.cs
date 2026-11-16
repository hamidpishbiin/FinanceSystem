namespace FinanceSystem.BackgroundService;

public sealed class ConnectionStrings
{
    public const string SectionName = "ConnectionStrings";
    public string DefaultConnection { get; set; } = default!;
    public string ReadOnlyConnection { get; set; } = default!;
}
