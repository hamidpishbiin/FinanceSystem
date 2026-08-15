# RabbitMQ

RabbitMQ is used as the asynchronous messaging infrastructure.

The Product module publishes integration events to RabbitMQ.

Other modules can consume these events without directly depending on
the Product module.

RabbitMQ is not used for synchronous request/response communication.

The goal of using RabbitMQ is to reduce coupling between modules and
prepare the system for future microservice extraction.