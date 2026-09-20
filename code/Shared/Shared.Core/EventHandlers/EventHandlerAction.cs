namespace Shared.Core.EventHandlers
{
    public class EventHandlerAction<T>(Action<T> action) : IEventHandler<T>
    {
        public async Task Handle(T domainEvent)
        {
            action.Invoke(domainEvent);
            await Task.FromResult(0);
        }
    }
}
