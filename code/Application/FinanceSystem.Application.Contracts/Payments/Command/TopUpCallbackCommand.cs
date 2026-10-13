using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using Shared.Core;

namespace FinanceSystem.Application.Contracts.Payments.Command;

public class TopUpCallbackCommand : ICommand
{
    public PspCode PspCode { get; set; }
    public IReadOnlyDictionary<string, string> Fields { get; set; } = new Dictionary<string, string>();
}
