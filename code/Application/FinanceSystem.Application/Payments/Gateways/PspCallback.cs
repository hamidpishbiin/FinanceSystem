using FinanceSystem.Domain.Payments.Enums;

namespace FinanceSystem.Application.Payments.Gateways;

public class PspCallback
{
    public long ReservationNumber { get; set; }
    public bool IsSuccess { get; set; }
    public string Status { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public string Token { get; set; } = string.Empty;
    public string ReferenceNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string MaskedPan { get; set; } = string.Empty;
    public string RRN { get; set; } = string.Empty;
    public string TraceNumber { get; set; } = string.Empty;
}
