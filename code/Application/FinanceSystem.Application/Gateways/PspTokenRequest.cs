namespace FinanceSystem.Application.Gateways;

public class PspTokenRequest()
{
    public required decimal Amount { get; set; }
    public required long ReservationNumber { get; set; }
    public required string CallbackUrl { get; set; }
    public string? AdditionalData { get; set; }
}
