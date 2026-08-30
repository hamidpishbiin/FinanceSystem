using ProductManagement.Domain.Payments;
using Shared.Application;
using Shared.Core;
using Shared.Core.EventHandlers;
using Shared.Core.Events;

namespace ProductManagement.Application.Payments.CommandHandlers;

public abstract class PaymentCommandHandler<T, TEvent> : BaseCommandHandler<T, TEvent>
    where TEvent : IDomainEvent
    where T : ICommand
{
    protected readonly IPaymentRepository Repository;

    protected PaymentCommandHandler(
        IPaymentRepository repository,
        IEventPublisher publisher,
        IEventListener listener,
        IEventHandler<TEvent> eventHandler)
        : base(publisher, listener, eventHandler)
    {
        Repository = repository;
    }
}
