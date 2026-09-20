namespace Shared.Core.Events
{
    public abstract record DomainEvent : IDomainEvent
    {
        public Guid EventId { get; }
        public DateTime CreatedAtUtc { get; }

        protected DomainEvent()
        {
            EventId = Guid.NewGuid();
            CreatedAtUtc = DateTime.UtcNow;
        }
    }
}
