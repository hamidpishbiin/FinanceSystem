using FinanceSystem.Domain.Bills;

namespace FinanceSystem.Application.Bills.CommandHandlers;

public abstract class BillCommandHandler<T, TEvent> : BaseCommandHandler<T, TEvent> where T : ICommand where TEvent : IDomainEvent
{
    protected readonly IBillRepository Repository;

    protected BillCommandHandler(
        IEventPublisher publisher,
        IEventListener listener,
        IEventHandler<TEvent> eventHandler,
        IBillRepository repository)
        : base(publisher, listener, eventHandler)
    {
        Repository = repository;
    }
}
