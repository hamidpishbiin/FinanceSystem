# Outbox Pattern

The Product module uses the Outbox Pattern to reliably publish events.

When an important business operation occurs, the domain event is stored
in PostgreSQL as part of the same database transaction as the business
data.

A background process later reads pending events from the Outbox table
and publishes them to RabbitMQ.

After successful publication, the event is marked as processed.

This prevents a situation where the database transaction succeeds but
the corresponding message is never published to RabbitMQ.