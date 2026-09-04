\# AI Guidelines



\## Purpose



This document defines how AI assistants must behave while generating or modifying code in this repository.



AI-generated code must follow the existing project architecture and coding standards.



\---



\# Architecture Rules



This project follows:



\* DDD

\* CQRS

\* Clean Architecture

\* Repository Pattern



AI assistants must always follow the existing architecture.



Do not introduce new architectural patterns unless explicitly requested.



\---



\# Controller Rules



RULE:

Controllers must remain thin.



Controllers are responsible only for:



\* receiving requests

\* validating input

\* orchestrating services



Controllers must never contain:



\* business logic

\* database access

\* repository usage



\---



\# DTO Rules



RULE:

All GET APIs must return DTOs.



RULE:

Entities must never be returned directly from APIs.



RULE:

DTOs must not contain business logic.



\---



\# Query Rules



RULE:

Query operations must never modify database state.



RULE:

Query services must be implemented inside:

FinanceSystem.Interface.ReadModel



\---



\# Write Rules



RULE:

Write operations are allowed to modify database state.



RULE:

Write services must be implemented inside:

FinanceSystem.Interface.WriteModel



\---



\# Dependency Rules



Forbidden:



\* Infrastructure depending on Presentation

\* Controllers using repositories directly

\* Business logic inside DTOs

\* Static helper business classes



\---



\# Code Style



AI-generated code must:



\* use async/await

\* use dependency injection

\* use meaningful naming

\* follow existing naming conventions

\* avoid unnecessary abstractions



\---



\# Preferred Patterns



Preferred:



\* constructor injection

\* DTO mapping

\* service orchestration

\* repository encapsulation

\* thin controllers



\---



\# Forbidden Behaviors



Forbidden:



\* generating fake implementations

\* changing unrelated files

\* introducing breaking changes

\* creating duplicate DTOs

\* bypassing validation



\---



\# Refactoring Rules



When refactoring:



\* preserve public contracts

\* preserve API response structure

\* avoid changing architecture

\* maintain backward compatibility



\---



\# Testing Rules



Generated tests should:



\* use xUnit

\* use Moq

\* follow Arrange / Act / Assert pattern



