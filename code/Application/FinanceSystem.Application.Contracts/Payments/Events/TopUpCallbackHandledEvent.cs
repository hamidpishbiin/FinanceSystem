using Shared.Core.Events;

namespace FinanceSystem.Application.Contracts.Payments.Events;

public record TopUpCallbackHandledEvent(TopUpOutcome Outcome, string ResultPageUrl) : DomainEvent;
