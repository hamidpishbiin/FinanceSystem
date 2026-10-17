using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Domain.Contract.Payments;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;

namespace FinanceSystem.Application.Payments.CommandHandlers;

public class PaymentCreateCommandHandler : PaymentCommandHandler<CreatePaymentCommand, PaymentCreatedEvent>
{
    public PaymentCreateCommandHandler(
        IPaymentRepository repository,
        IEventPublisher publisher,
        IEventListener listener,
        IEventHandler<PaymentCreatedEvent> eventHandler)
        : base(repository, publisher, listener, eventHandler)
    {
    }

    public override async Task Execute(CreatePaymentCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
