\# AI Coding Guidelines



\## Architecture



This project follows:



\* DDD

\* CQRS

\* Clean Architecture

\* Repository Pattern



AI assistants must always follow the existing architecture.



\---



\# Controller Rules



RULE:

Controllers must not contain business logic.



RULE:

Controllers are only responsible for:



\* request validation

\* orchestration

\* calling facade services



RULE:

Controllers containing the word `Query` are read-only controllers.

Only safe HTTP methods are allowed:



\* GET

\* HEAD



RULE:

Controllers without `Query` in the name are write controllers.

Allowed methods:



\* POST

\* PUT

\* DELETE



\---



\# API Response Rules



RULE:

All APIs must return:



\* JsonResponse<T>

\* JsonResponsePagination<List<T>>



RULE:

Entities must never be returned directly from APIs.



RULE:

All GET APIs must return DTOs.



\---



\# Interface Layer Rules



`FinanceSystem.Interface.Contracts` contains:



\* Interfaces

\* DTOs

\* Request models



Each entity module contains:



\* Services

\* DTOs

\* Models



\---



\# FacadeQuery Rules



RULE:

Interfaces containing `FacadeQuery` are read-only services.



RULE:

FacadeQuery implementations exist in:

`FinanceSystem.Interface.ReadModel`



RULE:

FacadeQuery methods must never modify database state.



\---



\# FacadeService Rules



RULE:

Interfaces containing `FacadeService` are write services.



RULE:

FacadeService implementations exist in:

`FinanceSystem.Interface.WriteModel`



RULE:

FacadeService methods are allowed to modify database state.



\---



\# DTO Rules



DTOs are located in:

`DTOs/`



RULE:

DTOs are used only for returning data from read operations.



\---



\# Request Model Rules



Request models are located in:

`Models/`



RULE:

Models represent incoming HTTP request bodies for write operations.



