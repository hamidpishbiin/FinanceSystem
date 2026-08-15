# System Architecture

Our application is an ASP.NET Core based backend system.

The system follows Clean Architecture and Domain-Driven Design principles.

The application is currently a modular monolith, but the architecture
is being prepared for future migration to microservices.

The main infrastructure components are:

- PostgreSQL for relational data
- Redis for caching
- RabbitMQ for asynchronous communication

The Product module is responsible for managing products and publishing
events when important business operations occur.