namespace FinanceSystem.Domain.Contract.Payments;

public record PaymentCreatedEvent(string IdempotencyKey, decimal AmountRial)
    : PaymentEventBase(IdempotencyKey, AmountRial);
