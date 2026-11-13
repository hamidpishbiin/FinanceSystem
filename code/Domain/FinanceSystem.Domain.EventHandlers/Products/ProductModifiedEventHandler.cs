using FinanceSystem.Domain.Contract.Products;

namespace FinanceSystem.Domain.EventHandlers.Products
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
