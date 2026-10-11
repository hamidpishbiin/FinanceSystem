using FinanceSystem.Domain.Payments.Enums;

namespace FinanceSystem.Application.Payments.Gateways;

public class PspCallback
{
    public long ReservationNumber { get; set; }
    public bool IsSuccess { get; set; }
    public PspFailureReason? FailureReason { get; set; }
    public string RawStatus { get; set; } = string.Empty;
    public int? RawStatusCode { get; set; }
    public string? Token { get; set; }
    public string? ReferenceNumber { get; set; }
    public decimal? Amount { get; set; }
    public string? MaskedPan { get; set; }
    public string? RRN { get; set; }
    public string? TraceNumber { get; set; }
}
