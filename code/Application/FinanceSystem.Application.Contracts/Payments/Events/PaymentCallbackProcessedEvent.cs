using Shared.Core.Events;

namespace FinanceSystem.Application.Contracts.Payments.Events;

public record PaymentCallbackProcessedEvent(PaymentOutcome Outcome, string ResultPageUrl) : DomainEvent;
