using FinanceSystem.Domain.Contract.Products;

namespace FinanceSystem.Domain.EventHandlers.ProductEventHandlers
{
    public class ProductCreatedEventHandler : ProductEventsHandlerBase<ProductCreated>
    {
        public ProductCreatedEventHandler(IProductEventRepository eventRepository) : base(eventRepository)
        {
        }

        protected override string DetermineEventType()
        {
            return "Created";
        }
    }
}
