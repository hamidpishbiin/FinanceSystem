using Shared.Core;

namespace FinanceSystem.Application.Contracts.Payments.Command;

public class CompleteTopUpProcessCommand : ICommand
{
    public int PspCode { get; set; }
    public long ReservationNumber { get; set; }
    public bool IsSuccess { get; set; }
    public string? RawStatus { get; set; }
    public int? RawStatusCode { get; set; }
    public string? Token { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? RRN { get; set; }
    public string? TraceNumber { get; set; }
    public string? MaskedPan { get; set; }
    public decimal? Amount { get; set; }
}
