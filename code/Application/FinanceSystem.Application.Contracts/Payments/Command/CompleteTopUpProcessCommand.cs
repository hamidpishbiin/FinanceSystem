using Shared.Core;

namespace FinanceSystem.Application.Contracts.Payments.Command;

public class CompleteTopUpProcessCommand : ICommand
{
    public int PspCode { get; set; }
    public IReadOnlyDictionary<string, string> Fields { get; set; } = new Dictionary<string, string>();
}
