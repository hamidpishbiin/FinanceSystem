namespace ProductManagement.Domain.Contract.Payments;

public class PaymentEventModel
{
    public Guid EventId { get; private set; }
    public DateTime HappenDateTime { get; private set; }
    public int EventState { get; private set; }
    public string EventType { get; private set; }

    public string EventBody { get; private set; }

    protected PaymentEventModel() { }

    public PaymentEventModel(PaymentEventBase domainEvent, Guid eventId, DateTime happenDateTime, string eventType)
    {
        EventId = eventId;
        HappenDateTime = happenDateTime;
        EventType = eventType;
        EventState = 0;
        EventBody = System.Text.Json.JsonSerializer.Serialize(new
        {
            domainEvent.IdempotencyKey,
            domainEvent.AmountRial,
        });
    }
}
