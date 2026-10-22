using Shared.Core;

namespace FinanceSystem.Application.Contracts.Payments.Command;

public abstract class PaymentCommand : ICommand
{
    public required long UserId { get; set; }
}
