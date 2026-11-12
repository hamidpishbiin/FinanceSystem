using FinanceSystem.Domain.Contract.Payments;

namespace FinanceSystem.Domain.EventHandlers.PaymentEventHandlers;

public class PaymentCreatedEventHandlerOld : PaymentEventsHandlerBase<PaymentCreated>
{
    public PaymentCreatedEventHandlerOld(IPaymentEventRepository eventRepository) : base(eventRepository)
    {
    }

    protected override string DetermineEventType()
    {
        return "Created";
    }
}
