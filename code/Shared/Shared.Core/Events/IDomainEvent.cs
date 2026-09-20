namespace Shared.Core.Events
{
    public interface IDomainEvent : IEvent
    {
        Guid EventId { get; }
        DateTime CreatedAtUtc { get; }
    }
}
