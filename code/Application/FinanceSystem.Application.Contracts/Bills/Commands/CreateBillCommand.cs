using FinanceSystem.Application.Contracts.Payments.Command;

namespace FinanceSystem.Application.Contracts.Bills.Commands;

public class CreateBillCommand : BillCommand
{
    public long ServiceId { get; set; }
    public long ReferenceId { get; set; }
    public decimal Amount { get; set; }
}
