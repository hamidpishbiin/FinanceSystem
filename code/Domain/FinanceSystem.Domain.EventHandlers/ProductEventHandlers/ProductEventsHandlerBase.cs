using FinanceSystem.Domain.Contract.Products;
using Shared.Core.EventHandlers;

namespace FinanceSystem.Domain.EventHandlers.ProductEventHandlers
{
    public abstract class ProductEventsHandlerBase<T> : IEventHandler<T> where T : ProductEventBase
    {
        private readonly IProductEventRepository _eventRepository;

        protected ProductEventsHandlerBase(IProductEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        protected abstract string DetermineEventType();

        public async Task Handle(T happen)
        {
            var model = new ProductEventModel(happen, happen.EventId, happen.CreateDateTime, DetermineEventType());
            await _eventRepository.Persist(model);
        }
    }
}
