namespace FinanceSystem.Domain.Contract.Payments;

public record PaymentCreatedEvent(string IdempotencyKey, decimal Amount)
    : PaymentEventBase(IdempotencyKey, Amount);
