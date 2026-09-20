using Shared.Core;
using Shared.Core.EventHandlers;
using Shared.Core.Events;

namespace Shared.Application
{
    public abstract class BaseCommandHandler<T> : ICommandHandler<T> where T : ICommand
    {
        protected readonly IEventPublisher Publisher;
        protected readonly IEventListener Listener;

        protected BaseCommandHandler(IEventPublisher publisher, IEventListener listener)
        {
            Publisher = publisher;
            Listener = listener;
        }

        public abstract Task Handle(T command, CancellationToken cancellationToken = default);
    }
    public abstract class BaseCommandHandler<T, TEvent> : BaseCommandHandler<T> where TEvent : IDomainEvent where T : ICommand
    {
        protected readonly IEventHandler<TEvent> EventHandler;

        protected BaseCommandHandler(
            IEventPublisher publisher,
            IEventListener listener,
            IEventHandler<TEvent> eventHandler) : base(publisher, listener)
        {
            EventHandler = eventHandler;
        }

        public override async Task Handle(T command, CancellationToken cancellationToken = default)
        {
            await Listener.Subscribe(EventHandler);
            await Execute(command, cancellationToken);
        }

        public abstract Task Execute(T command, CancellationToken cancellationToken = default);
    }
}
