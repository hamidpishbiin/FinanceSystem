using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Domain.Contract.Payments;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;

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
        var model = await Payment.Create(
            command.IdempotencyKey,
            (PaymentPurpose)command.Purpose,
            (PaymentChannel)command.Channel,
            new Money(command.AmountRial),
            command.SourceAccountId,
            command.DestinationAccountId,
            command.OriginServiceId,
            command.ExternalReferenceId,
            command.ExternalTag,
            command.PspPaymentDetailId,
            Publisher);

        await Repository.AddAsync(model);
    }
}
