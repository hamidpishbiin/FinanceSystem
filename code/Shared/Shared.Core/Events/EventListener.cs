using Shared.Core.EventHandlers;

namespace Shared.Core.Events
{
    public class EventListener(IEventAggregator eventAggregator) : IEventListener
    {
        public async Task Subscribe<T>(IEventHandler<T> eventHandler) where T : IDomainEvent
        {
            await eventAggregator.Subscribe<T>(eventHandler);
        }
    }
}
