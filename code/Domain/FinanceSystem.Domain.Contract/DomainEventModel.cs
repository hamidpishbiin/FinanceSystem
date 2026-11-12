using FinanceSystem.Domain.Contract.Exceptions;
using Shared.Core.Exceptions;

namespace FinanceSystem.Domain.Contract;

public class DomainEventModel
{
    public Guid EventId { get; private set; }
    public DateTime HappenDateTime { get; private set; }
    public DomainEventState EventState { get; private set; }
    public string EntityName { get; private set; }
    public string EventType { get; private set; }
    public string EventBody { get; private set; }

    private DomainEventModel(){}

    private DomainEventModel(
        Guid eventId,
        DateTime happenDateTime,
        DomainEventState eventState,
        string entityName,
        string eventType,
        string eventBody)
    {
        Guard<InvalidEventIdException>.IsTrue(eventId == Guid.Empty);
        Guard<InvalidHappenDateTimeException>.AgainstNull(happenDateTime);
        Guard<InvalidEventStateException>.IsFalse(Enum.IsDefined(eventState));
        Guard<InvalidEntityNameException>.AgainstNullOrEmpty(entityName);
        Guard<InvalidEventTypeException>.AgainstNullOrEmpty(eventType);
        Guard<InvalidEventBodyException>.AgainstNullOrEmpty(eventBody);

        EventId = eventId;
        HappenDateTime = happenDateTime;
        EventState = eventState;
        EntityName = entityName;
        EventType = eventType;
        EventBody = eventBody;
    }

    public static DomainEventModel CreatePending(
        Guid eventId,
        DateTime happenDateTime,
        string entityName,
        string eventType,
        string eventBody)
    {
        return new DomainEventModel(
            eventId,
            happenDateTime,
            DomainEventState.Pending,
            entityName,
            eventType,
            eventBody);
    }
}
