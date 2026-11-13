using System.Text.Json;
using FinanceSystem.Domain.Contract;
using Shared.Core.EventHandlers;
using Shared.Core.Events;

namespace FinanceSystem.Domain.EventHandlers;

public abstract class EventHandlerBase<T>(
    IDomainEventRepository domainEventRepository)
    : IEventHandler<T> where T : IDomainEvent
{
    protected virtual string DetermineEventType() => typeof(T).Name;
    protected virtual string DetermineEventBody(T happen) => JsonSerializer.Serialize(happen);
    protected abstract string DetermineEntityName();

    public async Task Handle(T happen)
    {
        var domainEvent = DomainEventModel.CreatePending(
            happen.EventId,
            happen.CreatedAtUtc,
            DetermineEntityName(),
            DetermineEventType(),
            DetermineEventBody(happen));

        await domainEventRepository.AddAsync(domainEvent, CancellationToken.None);
    }
}
