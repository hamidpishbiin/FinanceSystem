using Shared.Core.Events;

namespace FinanceSystem.Application.Contracts.Payments.Events;

public record TopUpCallbackHandled(TopUpOutcome Outcome, string ResultPageUrl) : DomainEvent;
