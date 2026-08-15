# Architecture Decision 001

RabbitMQ will be used only for integration events, while internal domain events must remain inside the Product module.

## Facts

- RabbitMQ will be used only for integration events.  
  Source: `architecture-decision-001.md`
- Internal domain events must remain inside the Product module and must not be published directly to RabbitMQ.  
  Source: `architecture-decision-001.md`

## Relationships


## Unknown / Not Specified
