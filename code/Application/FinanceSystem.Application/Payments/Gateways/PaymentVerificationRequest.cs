namespace FinanceSystem.Application.Payments.Gateways;

public class PaymentVerificationRequest
{
    public long ReservationNumber { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}
