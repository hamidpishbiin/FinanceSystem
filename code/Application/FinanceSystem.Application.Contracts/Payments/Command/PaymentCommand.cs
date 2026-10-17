using Shared.Core;

namespace FinanceSystem.Application.Contracts.Payments.Command;

public abstract class PaymentCommand : ICommand
{
    public long Token { get; set; }
}
