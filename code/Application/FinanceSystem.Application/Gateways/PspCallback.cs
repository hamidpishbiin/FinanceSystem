using FinanceSystem.Domain.Payments;

namespace FinanceSystem.Application.Gateways;

public class PspCallback
{
    public long ReservationNumber { get; set; }
    public bool IsSuccess { get; set; }
    public string Token { get; set; } = string.Empty;
    public string ReferenceNumber { get; set; } = string.Empty;
    public Money Amount { get; set; }
    public string MaskedPan { get; set; } = string.Empty;
    public string RRN { get; set; } = string.Empty;
    public string TraceNumber { get; set; } = string.Empty;
}
