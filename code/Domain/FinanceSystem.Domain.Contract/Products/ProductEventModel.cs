namespace FinanceSystem.Domain.Contract.Products
{
    public class ProductEventModel
    {
        public Guid EventId { get; private set; }
        public DateTime HappenDateTime { get; private set; }
        public int EventState { get; private set; }
        public string EventType { get; private set; }

        public string EventBody { get; private set; }

        protected ProductEventModel() { }

        public ProductEventModel(ProductEventBase domainEvent, Guid eventId, DateTime happenDateTime, string eventType)
        {
            EventId = eventId;
            HappenDateTime = happenDateTime;
            EventType = eventType;
            EventState = 0;
            EventBody = System.Text.Json.JsonSerializer.Serialize(new 
            {
                domainEvent.Id,
                domainEvent.Name,
                domainEvent.Description,
                domainEvent.Price,
                domainEvent.StockQuantity,
            });
        }
    }
}
