using FinanceSystem.Domain.Contract.Payments;
using Shared.Core.EventHandlers;

namespace FinanceSystem.Domain.EventHandlers.PaymentEventHandlers;

public abstract class PaymentEventsHandlerBase<T> : IEventHandler<T> where T : PaymentEventBase
{
    private readonly IPaymentEventRepository _eventRepository;

    protected PaymentEventsHandlerBase(IPaymentEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    protected abstract string DetermineEventType();

    public async Task Handle(T happen)
    {
        var model = new PaymentEventModel(happen, happen.EventId, happen.CreateDateTime, DetermineEventType());
        await _eventRepository.Persist(model);
    }
}
