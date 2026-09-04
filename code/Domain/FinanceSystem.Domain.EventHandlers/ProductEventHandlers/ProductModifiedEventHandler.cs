using FinanceSystem.Domain.Contract.Products;

namespace FinanceSystem.Domain.EventHandlers.ProductEventHandlers
{
    public class ProductModifiedEventHandler : ProductEventsHandlerBase<ProductModified>
    {
        public ProductModifiedEventHandler(IProductEventRepository eventRepository) : base(eventRepository)
        {
        }

        protected override string DetermineEventType()
        {
            return "Modified";
        }
    }
}
