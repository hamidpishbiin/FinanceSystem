using FinanceSystem.Domain.Payments.Enums;

namespace FinanceSystem.Application.Payments.Gateways;

public class PspTokenRequest()
{
    public required decimal Amount { get; set; }
    public required long ReservationNumber { get; set; }
    public required PaymentPurpose PaymentPurpose { get; set; }
    public string? AdditionalData { get; set; }
}
