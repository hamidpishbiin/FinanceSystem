\# CQRS Pattern



\## Overview



This project follows CQRS (Command Query Responsibility Segregation).



Read and write operations are completely separated.



\---



\# Query Side



Query operations:



\* only retrieve data

\* never modify database state

\* return DTOs only



Query controllers contain:



\* Query in controller name



Examples:



\* ProductQueryController

\* OrderQueryController



\---



\# Query Service Rules



Interfaces containing:



FacadeQuery



represent read-only services.



Implementations exist in:



FinanceSystem.Interface.ReadModel



\---



\# Command Side



Command operations:



\* modify database state

\* execute business logic

\* validate requests



Command controllers do NOT contain:



Query



in their name.



Examples:



\* ProductController

\* OrderController



\---



\# Command Service Rules



Interfaces containing:



FacadeService



represent write services.



Implementations exist in:



FinanceSystem.Interface.WriteModel



\---



\# DTO Rules



DTOs are used only for read operations.



DTOs must never contain business logic.



\---



\# Request Models



Request models represent incoming HTTP body payloads.



Located in:

Models/



\---



\# Forbidden Patterns



Forbidden:



\* business logic inside controllers

\* repositories inside controllers

\* entities returned directly from APIs

\* database access inside query DTOs



\---



\# Recommended Flow



HTTP Request

→ Controller

→ FacadeService / FacadeQuery

→ Repository

→ Database



