using Shared.Core;

namespace FinanceSystem.Application.Contracts.Bills.Commands;

public abstract class BillCommand : ICommand
{
    public long UserId { get; set; }
}
