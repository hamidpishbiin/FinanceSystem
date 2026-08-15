# Architecture Decision 001

The team decided that RabbitMQ will be used only for
integration events.

Internal domain events must remain inside the Product module
and must not be published directly to RabbitMQ.

The Outbox Pattern remains responsible for reliably publishing
integration events.