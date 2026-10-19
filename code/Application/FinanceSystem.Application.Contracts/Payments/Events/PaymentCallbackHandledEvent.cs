using Shared.Core.Events;

namespace FinanceSystem.Application.Contracts.Payments.Events;

public record PaymentCallbackHandledEvent(PaymentOutcome Outcome, string ResultPageUrl) : DomainEvent;
