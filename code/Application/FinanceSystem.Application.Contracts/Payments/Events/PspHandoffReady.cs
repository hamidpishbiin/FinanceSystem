using Shared.Core.Events;

namespace FinanceSystem.Application.Contracts.Payments.Events;

public record PspHandoffReady(PspHandoff Handoff) : DomainEvent;
