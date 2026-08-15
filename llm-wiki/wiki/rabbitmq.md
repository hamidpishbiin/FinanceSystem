# RabbitMQ

RabbitMQ is used as the asynchronous messaging infrastructure, with the Product module publishing integration events to RabbitMQ. Other modules can consume these events without directly depending on the Product module.

## Facts

- RabbitMQ is used as the asynchronous messaging infrastructure.  
  Source: `rabbitmq.md`
- The Product module publishes integration events to RabbitMQ.  
  Source: `rabbitmq.md`, `outbox.md`
- Other modules can consume these events without directly depending on the Product module.  
  Source: `rabbitmq.md`

## Relationships

- **Product module** → **publishes integration events to** → **RabbitMQ**  
  Source: `rabbitmq.md`, `outbox.md`

## Unknown / Not Specified
