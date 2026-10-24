using FinanceSystem.Domain.Bills;

namespace FinanceSystem.Application.Bills.CommandHandlers;

public abstract class BillCommandHandler<T> : BaseCommandHandler<T> where T : ICommand
{
    protected readonly IBillRepository Repository;

    protected BillCommandHandler(
        IBillRepository billRepository,
        IEventPublisher publisher,
        IEventListener listener) : base(publisher, listener)
    {
        Repository = billRepository;
    }
}

public abstract class BillCommandHandler<T, TEvent> : BaseCommandHandler<T, TEvent> where T : ICommand where TEvent : IDomainEvent
{
    protected readonly IBillRepository Repository;

    protected BillCommandHandler(
        IBillRepository repository,
        IEventPublisher publisher,
        IEventListener listener,
        IEventHandler<TEvent> eventHandler)
        : base(publisher, listener, eventHandler)
    {
        Repository = repository;
    }
}

