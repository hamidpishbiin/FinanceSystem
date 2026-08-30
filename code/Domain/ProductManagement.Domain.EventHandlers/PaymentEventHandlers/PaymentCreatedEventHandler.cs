using ProductManagement.Domain.Contract.Payments;

namespace ProductManagement.Domain.EventHandlers.PaymentEventHandlers;

public class PaymentCreatedEventHandler : PaymentEventsHandlerBase<PaymentCreatedEvent>
{
    public PaymentCreatedEventHandler(IPaymentEventRepository eventRepository) : base(eventRepository)
    {
    }

    protected override string DetermineEventType()
    {
        return "Created";
    }
}
