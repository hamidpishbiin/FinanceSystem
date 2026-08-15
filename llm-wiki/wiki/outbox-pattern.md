# Outbox Pattern

The Product module uses the Outbox Pattern to reliably publish events by storing them in PostgreSQL during the same transaction as the business data, and a background process publishes these events to RabbitMQ.

## Facts

- The Product module uses the Outbox Pattern to reliably publish events.  
  Source: `outbox.md`
- When an important business operation occurs, the domain event is stored in PostgreSQL as part of the same database transaction as the business data.  
  Source: `outbox.md`
- A background process later reads pending events from the Outbox table and publishes them to RabbitMQ.  
  Source: `outbox.md`
- After successful publication, the event is marked as processed.  
  Source: `outbox.md`

## Relationships

- **Outbox Pattern** → **uses** → **PostgreSQL**  
  Source: `outbox.md`
- **Outbox Pattern** → **publishes events to** → **RabbitMQ**  
  Source: `rabbitmq.md`, `outbox.md`

## Unknown / Not Specified
