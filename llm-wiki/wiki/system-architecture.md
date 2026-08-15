# System Architecture

The application is an ASP.NET Core based backend system following Clean Architecture and Domain-Driven Design principles. It is a modular monolith with plans for future microservices migration.

## Facts

- Our application is an ASP.NET Core based backend system.  
  Source: `architecture.md`
- The system follows Clean Architecture and Domain-Driven Design principles.  
  Source: `architecture.md`
- The application is currently a modular monolith, but the architecture is being prepared for future migration to microservices.  
  Source: `architecture.md`

## Relationships

- **Product module** → **publishes events** → **RabbitMQ (for integration events)**  
  Source: `rabbitmq.md`, `outbox.md`
- **Outbox Pattern** → **uses** → **PostgreSQL**  
  Source: `outbox.md`

## Unknown / Not Specified

- Details on specific modules and their dependencies