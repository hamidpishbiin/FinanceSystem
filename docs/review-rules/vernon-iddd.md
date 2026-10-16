# Review rules — Vernon, *Implementing Domain-Driven Design* (2013)

Source: Vaughn Vernon, *Implementing Domain-Driven Design*,
`/Users/hamid/Dev/Books/Implementing Domain-Driven Design.pdf`. References in `[brackets]` are the book's
printed page numbers. A chapter range like `Ch10 [345–388]` is used where the exact page wasn't recorded —
open that range with `pdftotext -f N -l M` if the detail matters.

Chapter → printed pages: Ch1 [1–42] · Ch2 Domains/Contexts [43–84] · Ch3 Context Maps [85–110] ·
Ch4 Architecture [111–168] · Ch5 Entities [169–216] · Ch6 Value Objects [217–262] · Ch7 Services [263–284] ·
Ch8 Domain Events [285–332] · Ch9 Modules [333–344] · Ch10 Aggregates [345–388] · Ch11 Factories [389–400] ·
Ch12 Repositories [401–448] · Ch13 Integrating Contexts [449–508] · Ch14 Application [509–538] ·
Appendix A Aggregates + Event Sourcing [539–583].

**Role of this book in reviews:** practical implementation — how to size aggregates, write application
services/command handlers, publish and consume events, persist, and integrate. For "why does DDD care?",
cite Evans (`evans-ddd.md`). Where both books cover a rule, cite the one whose angle fits the finding
(Vernon for *how*, Evans for *why*); don't report the same defect twice.

These rules are paraphrased, not quoted. Each has an ID so findings can cite it, e.g. `VN-AGG-3`.

**Severity guide** (same as Evans)

- **Breaks** — violates a rule the book treats as structural. Report as a finding.
- **Smell** — the book names it as a warning sign; context may justify it. Report with the trade-off.
- **Learning note** — not a defect; an opportunity to apply a concept. Mention only when relevant.

---

## Project decisions that override the book

Recorded decisions win over these rules. Flag the tension once if it matters, never as a defect.

- **Repository interfaces live in Domain**, next to the aggregate. Matches Vernon (VN-REPO-2).
- **Product types are hands-off** (`Domain/.../Products/**`, `Domain.Contract/Products/**`,
  `ProductUnitTests/**`). Report, never edit.
- **Minimal first.** Vernon's heavier machinery (event store, notification feeds, process trackers,
  Data Transformers, event sourcing) is **not** to be recommended until a concrete need exists. Mention it
  as a Learning note at most.
- **PSP gateway settings live in `PspOptions` config**, not on `PaymentServiceProvider`. Settled.
- **No code comments / no XML doc comments.** Never suggest adding them.
- **`EventHandlerAction<T>` result capture in facades** is deliberate. It is close to Vernon's
  output-port idea (VN-APP-7); not a violation.
- **Database-generated `long` ids (late identity)** were chosen for `User`, `PaymentServiceProvider`
  and `Bill`; Product keeps `Guid`. Vernon prefers early identity when a creation event must carry the id
  (VN-ENT-3). The consequence — a `XCreated` event published from the constructor carries `Id = 0` — is
  still a real defect (VN-EVT-4); the fix is to publish after the id exists, not to change the id
  strategy.
- **Raw primitive ids** (`long`, `Guid`) instead of typed id value objects (`BillId`) are the current
  style. Typed ids (VN-VO-6) are a Learning note, not a finding.
- **No database yet / no outbox yet.** Same-transaction event persistence and an outbox for
  `Payment.Mark*` events are already recorded as pending work. Rules VN-EVT-7/8 are advisory until then;
  don't re-report the known gap on every review — only when a change makes it worse.
- **In-process `EventAggregator`** (synchronous, per lifetime scope) is the current event mechanism.
  It is Vernon's lightweight publisher (VN-EVT-5); the rule that matters is VN-EVT-6.

---

## 1. Strategic fit and bounded contexts  `VN-BC`

| ID | Rule | Why | Signs of violation in this codebase | Ref |
|---|---|---|---|---|
| VN-BC-1 | Use the tactical patterns where the domain is complex enough; simple CRUD areas may stay simple. | DDD is meant to simplify, not to decorate CRUD. | Elaborate aggregates/events around a lookup table with no rules. | Ch1 [1–42] |
| VN-BC-2 | A bounded context is a linguistic boundary sized to its language — not split by layer, deployment unit or developer task. Use modules/projects for those splits. | Splitting by technology fragments the language. | Proposing a new "context" just because something is a separate project or endpoint. | Ch2 [43–84], Ch4 [111–168] |
| VN-BC-3 | Identity/security concepts (users, permissions, roles) belong to their own context or the application layer; the core domain sees roles as domain values (payer, account owner). | Mixing them puts the wrong language in the core. | Domain entities reading claims, roles or `IUserResolver`; business methods checking permissions. | Ch2 [43–84] |
| VN-BC-4 | One concept with the same name in two contexts is two types. | False cognates. | One class reused with different meanings in Payments and Bills. | Ch2–3 [43–110] |
| VN-BC-5 | Map relationships as they really are today (conformist, ACL, open host…), not as hoped. | Hidden conformist relationships surprise you later. | Treating an external PSP's contract as if we controlled it. | Ch3 [85–110] |

## 2. Architecture  `VN-ARCH`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| VN-ARCH-1 | Every architectural element must be justified by a real quality requirement; remove what isn't. | Architecture as fashion adds cost. | Layers, buses or abstractions with a single pass-through implementation and no need behind them. | Ch4 [111–168] |
| VN-ARCH-2 | Dependency inversion: the domain defines interfaces (repositories, technical services); infrastructure implements them; the application depends only on the abstractions. | Keeps the core independent of technology. | Domain or Application projects referencing EF Core, HTTP clients, or Persistence types. | Ch4 [111–168], Ch14 [532–534] |
| VN-ARCH-3 | Design the application's API per use case, not per client type; adapters (controller, WCF, message listener) translate to it. | Each new host should be just a new adapter. | Use-case logic duplicated in controllers; facade methods shaped around one UI screen. | Ch4 [111–168] |
| VN-ARCH-4 | Don't expose the domain model directly as API resources; build representations per client use case. | Every model change would break clients. | Entities returned from controllers or serialised as responses. | Ch4 [111–168], Ch14 [516–517] |
| VN-ARCH-5 | CQRS: commands go through aggregates; queries may use a separate, denormalised read path. Adopt it only when views cut across many aggregates. | Otherwise it is accidental complexity. | Repository finders multiplying for screens (VN-REPO-7) — or a read model introduced for one simple view. | Ch4 [111–168] |
| VN-ARCH-6 | Command handlers never call other command handlers. | Hidden chaining defeats one-aggregate-per-transaction. | A handler that `Send`s another command through `ICommandBus` inside its own transaction. | Ch4 [111–168] |
| VN-ARCH-7 | UI validation never replaces domain validation. | The model must protect itself. | Rules only in request models / controller attributes, absent from the entity. | Ch4 [111–168] |

## 3. Application services and command handlers  `VN-APP`

In this codebase, command handlers (`BaseCommandHandler<T>`) **are** the application services, and the
`Interface.WriteModel` facades are a thin layer above them.

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| VN-APP-1 | One application-service method / one command handler per use-case flow. | Expresses use cases explicitly. | A general "save/update everything" command with optional fields. | Ch1 [1–42], Ch14 [521] |
| VN-APP-2 | Keep handlers thin: load aggregate(s) by id, call **one** domain command (or a factory), add to repository. Anything more is domain logic leaking. | Fat handlers make the model anemic. | Handlers computing amounts, choosing states, looping to sum values, or checking several business conditions. | Ch4 [111–168], Ch14 [521–527] |
| VN-APP-3 | A significant multi-step domain process belongs in a domain service, called once from the handler. | Keeps the steps inside the model. | A handler creating an aggregate, assigning related state and publishing several events itself. | Ch14 [526–527] |
| VN-APP-4 | Transactions and security are application concerns: set them at the handler/decorator boundary, never in the domain. | One transaction per use case. | `SaveChanges`/transactions in repositories or entities; permission checks inside entities. | Ch12 [401–448], Ch14 [521–526] |
| VN-APP-5 | Guard against a missing aggregate right after loading it, consistently. | A null aggregate shouldn't reach domain calls. | Handlers dereferencing a repository result without a not-found check; each handler inventing a different exception. | Ch14 [524] |
| VN-APP-6 | Command objects are named for the operation, carry simple data, and are the input to the handler. | A serialisable, explicit request. | Commands named generically (`BillCommand`), or carrying entities / EF types. | Ch14 [527–528], App A [549–550] |
| VN-APP-7 | Returning output from a void command is done via an output port (or returning the new id), not by exposing domain objects to clients. | Decouples clients from the model. | Facades returning domain entities. (`EventHandlerAction` capture is an accepted output port here.) | Ch14 [528–530] |
| VN-APP-8 | Cross-cutting behaviour (transactions, logging, auth, validation, retry) goes into decorators around a uniform handler interface. | One place for all aspects. | Logging/try-catch/transaction code repeated in each handler instead of a decorator like `TransactionalCommandHandlerDecorator`. | App A [551–553] |
| VN-APP-9 | Infrastructure dependencies reach handlers via injection or a service factory; aggregates never get them injected. | See VN-AGG-8. | — | Ch14 [533–537] |

## 4. Entities  `VN-ENT`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| VN-ENT-1 | Don't design entities from the database schema; most "columns" are values, and only things with individuality and lifecycle are entities. | DB-first modelling produces anemic getter/setter classes. | Classes that mirror tables 1:1 with public setters and no behaviour. | Ch5 [169–216] |
| VN-ENT-2 | Behaviour as intention-revealing methods (`Activate()`, `MarkPaid()`), not public setters. A use case needing several setters hides its intention and can't publish one meaningful event. | Anemic models lose intent. | `public set` on business properties (`FinanceAccountId`, `Status`, `Amount`); handlers setting several properties in sequence. | Ch1 [1–42], Ch5 [169–216] |
| VN-ENT-3 | Choose identity generation deliberately (user, application, database, other context) and its timing. Early identity is needed when the creation event must carry the id or new entities are compared by id. | Late (DB) ids are 0 until saved. | Creation events or sets/equality relying on an id that is still 0. | Ch5 [169–216] |
| VN-ENT-4 | Identity is stable: no public id setter; modify-once at most. | Identity defines the entity. | Settable `Id`; code reassigning ids. | Ch5 [169–216] |
| VN-ENT-5 | The constructor takes everything needed to identify the entity and satisfy its invariants, with guards (null, range, format, length). | Valid from birth. | Objects that need setters after `new` to become valid; missing null guards on reference parameters. | Ch5 [169–216] |
| VN-ENT-6 | Validation has three levels: attribute guards (in the entity/VO), whole-object checks (a validator/specification that collects all problems), and cross-aggregate checks (a domain service). | Each level has a different home. | Whole-object or cross-aggregate rules crammed into the constructor, or living in a handler. | Ch5 [169–216] |
| VN-ENT-7 | Hide internals clients shouldn't navigate into; put the command on the entity that owns the change. | Tell, don't ask. | Callers reaching through `a.B.C` to change state. | Ch5 [169–216] |
| VN-ENT-8 | Infrastructure-only members (ORM constructors, navigation properties) stay non-public. | Framework needs mustn't widen the model's API. | Public parameterless constructors or public navigation setters added for EF. | Ch5 [169–216], Ch6 [217–262] |

## 5. Value objects  `VN-VO`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| VN-VO-1 | Prefer values over entities; entities should mostly be containers of values. | Values are simpler, safer and cheaper to persist. | Child entities with ids where a value would do. | Ch6 [217–262] |
| VN-VO-2 | Values are immutable, conceptual wholes, compared by value, replaced not modified. | — | Mutable properties on a value; amount and currency held as separate entity fields. | Ch6 [217–262] |
| VN-VO-3 | Avoid primitive obsession for concepts in the language: wrap them in domain types that own their rules. | Rules otherwise leak to every caller. | `decimal Amount` on an entity while `Money` exists; formatting/range checks repeated at call sites. | Ch6 [217–262] |
| VN-VO-4 | Value methods are side-effect free and take values, not entities, as parameters. | Limits coupling; testable. | A value method taking an aggregate to read one field from it. | Ch6 [217–262] |
| VN-VO-5 | Persisting a value as a table doesn't make it an entity; map single values as owned columns and keep any surrogate key hidden. | The data model serves the domain model. | A value given a public `Id` because EF stores it in its own table. | Ch6 [217–262] |
| VN-VO-6 | Typed identity values (`BillId`, `PaymentId`) let the compiler catch swapped arguments. | Raw ids are interchangeable by accident. | Constructors taking several `long` ids in a row (`serviceId`, `referenceId`, `accountId`). *Learning note here* (see overrides). | Ch5 [169–216], Ch9 [333–344], App A [578–579] |
| VN-VO-7 | Standard types (type codes, statuses) are values — usually enums — in the consuming context; display text belongs to the UI. | Not every lookup is an aggregate. | Status lookup entities with repositories; enums carrying localised descriptions. | Ch6 [217–262] |
| VN-VO-8 | Over-wrapping trivial self-contained primitives is a lesser mistake, but still a mistake. | Balance. | VOs with no rules around a single bool. | Ch6 [217–262] |

## 6. Aggregates  `VN-AGG`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| VN-AGG-1 | Model **true invariants** in consistency boundaries: the aggregate is exactly what must be transactionally consistent. | Developer-imagined invariants create oversized clusters. | Collections inside an aggregate kept "for convenience" with no rule linking them. | Ch10 [345–388] |
| VN-AGG-2 | Design small aggregates: a root plus the values that must stay consistent with it. Many aggregates are a single entity plus values. | Fewer conflicts, better performance. | Loading a large graph to add one item. | Ch10 [345–388] |
| VN-AGG-3 | Reference other aggregates by identity, not by object reference. | Holding a reference tempts modifying both together. | Navigation properties to another aggregate root (`Bill.FinanceAccount`) used for behaviour rather than just an id. | Ch10 [345–388] |
| VN-AGG-4 | Modify **one aggregate instance per transaction**; other aggregates are updated eventually via domain events. | Cross-aggregate transactions cause contention and coupling. | One handler changing a `Payment` and a `FinanceAccount` in the same transaction. Check with the "whose job is it?" test: the current user's job → may be transactional; someone else's or the system's → eventual. | Ch10 [345–388] |
| VN-AGG-5 | Breaking the one-aggregate rule needs a named reason (UI batch-creating new aggregates, no async mechanism yet, global transaction mandate, query performance). | Exceptions must be conscious. | Multi-aggregate commits with no stated reason. | Ch10 [345–388] |
| VN-AGG-6 | Commands go to the root; inner parts' mutators are not public; collections are exposed read-only. | Law of Demeter / tell, don't ask. | Public `List<T>` on a root; inner entity setters callable from handlers. | Ch10 [345–388] |
| VN-AGG-7 | Optimistic concurrency versions the root; changes to inner parts must still bump it. | Otherwise concurrent edits slip through. | A concurrency token on the root while inner entities change without touching it. | Ch10 [345–388] |
| VN-AGG-8 | Don't inject repositories or services into aggregates. Load what's needed first and pass it in, or pass a domain service into the method (double dispatch). | Keeps aggregates persistence-ignorant and testable. | Aggregates holding service/repository references. (`IEventPublisher Publisher` on aggregates is this codebase's publisher hook — see VN-EVT-5.) | Ch10 [345–388] |
| VN-AGG-9 | A use case that seems to need several aggregates in one transaction may reveal a missing concept, or should be re-specified as eventually consistent with an agreed delay. | Use cases aren't always right about transactions. | — | Ch10 [345–388] |

## 7. Factories  `VN-FAC`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| VN-FAC-1 | When one aggregate naturally creates another, put a factory method on the creating root (`account.IssueBill(...)`). It speaks the language, sets the association ids correctly, enforces preconditions, and publishes the creation event. | Clients can't get the association wrong. | Handlers `new`-ing an aggregate and assigning its parent id by hand. | Ch11 [389–400] |
| VN-FAC-2 | If a factory exists, make the created type's constructor non-public. | Forces the intended path. | Public constructor alongside a factory method. | Ch11 [389–400] |
| VN-FAC-3 | The application service still adds the new aggregate to its repository. | Factories create; repositories store. | Factories calling repositories. | Ch11 [389–400] |
| VN-FAC-4 | A domain-service factory can build local values from another context (adapter + translator behind a domain interface). | Keeps foreign terms out. | — | Ch11 [389–400] |

## 8. Domain services  `VN-SVC`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| VN-SVC-1 | Use a domain service for a significant process, a transformation, or a calculation needing input from more than one aggregate. Stateless, named in the language. | Doesn't fit any single object. | Static methods on an aggregate that take other aggregates. | Ch7 [263–284] |
| VN-SVC-2 | Domain services may use repositories; aggregates may not. | — | — | Ch7 [263–284] |
| VN-SVC-3 | Don't overuse services; behaviour that belongs to one entity stays there. | Anemic model. | A `XxxService` that only reads one entity and sets its state. | Ch7 [263–284] |
| VN-SVC-4 | Domain-specific steps never leak into clients: the handler calls one domain operation, not its sub-steps. Return a small value rather than an entity when that's all the client needs. | Domain knowledge stays in the model. | Handler doing "check active → find → check enabled → act" itself. | Ch7 [263–284] |
| VN-SVC-5 | A separated interface only when there are several (or technical) implementations; `XxxImpl` naming suggests the interface isn't needed. Technical implementations live in infrastructure. | Avoids ceremony. | `IFoo`/`FooImpl` pairs with one implementation in the domain. | Ch7 [263–284] |
| VN-SVC-6 | Even trivial calculations (sums, totals) are domain logic — not in handlers. | Small leaks add up. | LINQ `Sum` over domain data inside a handler. | Ch7 [263–284] |
| VN-SVC-7 | Expected domain outcomes (failed match, not eligible) needn't be exceptions; reserve exceptions for the exceptional. | Clarity of flow. | Exceptions used for normal branching. | Ch7 [263–284] |

## 9. Domain events  `VN-EVT`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| VN-EVT-1 | Model events the business cares about ("when…", "if that happens…"); name them past-tense from the command in the context's language. | Events are part of the language. | Technical names (`BillSaved`, `EntityUpdated`). | Ch8 [285–332] |
| VN-EVT-2 | Events are immutable: set in the constructor, read-only properties, any methods side-effect free. | Facts don't change. | Events with public setters. | Ch8 [285–332], App A [577] |
| VN-EVT-3 | Events carry when it happened, the identities involved, and enough data for most subscribers (~80%) — ids plus display names — so they don't query back. | Avoids chatty subscribers. | Events carrying only an id that every handler then reloads; or entire entities. | Ch8 [285–332], App A [573–575] |
| VN-EVT-4 | The aggregate publishes its own event as the last step of a successful command — never the client, and never before the data it reports exists. | Publishing from clients leaks logic; early publishing states something false. | Handlers publishing domain events; creation events carrying `Id = 0`. | Ch1 [1–42], Ch8 [285–332] |
| VN-EVT-5 | The domain publisher is a lightweight in-process publish/subscribe with no messaging-infrastructure dependency. | Domain stays technology-free. | Domain referencing a broker client. | Ch8 [285–332] |
| VN-EVT-6 | An in-process, same-transaction event handler must not modify **another aggregate**. Cross-aggregate reactions run in their own transaction (asynchronously). | Otherwise it's a hidden multi-aggregate transaction. | `IEventHandler<T>` implementations loading and changing another aggregate while the original command's transaction is open. | Ch8 [285–332], Ch10 [345–388] |
| VN-EVT-7 | When events must leave the process, store them in the same local transaction as the aggregate change (event store / outbox) and forward them later. | No dual-write inconsistency. | Publishing to an external system before or after commit without a stored record. *Advisory until the outbox lands.* | Ch8 [285–332] |
| VN-EVT-8 | Subscribers must cope with duplicates and out-of-order delivery: idempotent operations, or record handled message ids in the same transaction. | At-least-once delivery. | Handlers that double-apply on redelivery (e.g. crediting twice). | Ch8 [285–332], Ch13 [449–508] |
| VN-EVT-9 | Not every command needs an event — only those that matter to the business or to other parts. | Avoid noise. | Events nobody subscribes to and nobody asked for. | Ch8 [285–332] |
| VN-EVT-10 | A subscriber in the same context delegates to an application service/command, which owns the transaction and calls the aggregate. | Same structure as any use case. | Event handlers containing business logic directly. | Ch8 [285–332], Ch13 [449–508] |

## 10. Modules  `VN-MOD`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| VN-MOD-1 | Modules follow domain concepts (one aggregate or a few cohesive ones), named in the language; never by pattern type. | Pattern folders stifle modelling. | Folders like `Entities/`, `Exceptions/`, `Services/` at the top of the domain. | Ch9 [333–344] |
| VN-MOD-2 | Dependencies between peer modules are acyclic. | Cycles weld modules together. | `Bills` using `Payments` while `Payments` uses `Bills`. | Ch9 [333–344] |
| VN-MOD-3 | When a linguistic boundary is unclear, separate with a module before inventing a new context. | Cheaper and reversible. | — | Ch9 [333–344] |

## 11. Repositories  `VN-REPO`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| VN-REPO-1 | Only aggregates get repositories, usually one per aggregate type. | Entry points only. | Repositories for child entities or values. | Ch12 [401–448] |
| VN-REPO-2 | Collection-oriented style fits EF Core's change tracker: add once, no re-save after modification; interface in the aggregate's module, implementation in infrastructure. | Matches unit-of-work semantics. | Explicit `Update()` calls after modifying a tracked aggregate. | Ch12 [401–448] |
| VN-REPO-3 | Repositories never control transactions; the application layer does. Don't commit multiple aggregates in one transaction just because tests pass. | Production concurrency will expose it. | `SaveChanges` inside repository methods. | Ch12 [401–448] |
| VN-REPO-4 | Finders are named in domain terms; tenant/owner filters always apply. | Intention-revealing. | `GetByColumnX` names. | Ch12 [401–448] |
| VN-REPO-5 | Translate persistence exceptions (unique violations, etc.) into domain/application exceptions. | ORM errors shouldn't leak. | `DbUpdateException` surfacing to API clients. | Ch12 [401–448] |
| VN-REPO-6 | Don't offer `Remove` if the business forbids deletion; use a state (disabled/cancelled). | Deletion is a domain decision. | Hard deletes of financial records. | Ch12 [401–448] |
| VN-REPO-7 | A use-case-optimal finder returning a value/view is acceptable; many of them across repositories signals mis-sized aggregates or a need for CQRS. | Smell detector. | Repositories growing screen-shaped query methods. | Ch12 [401–448], Ch14 [517] |
| VN-REPO-8 | Repository vs DAO: no fine-grained CRUD over aggregate parts; no business logic in stored procedures. | Logic hidden from the model. | Methods updating single child rows. | Ch12 [401–448] |
| VN-REPO-9 | Test real repository implementations against a real store; use in-memory fakes to test handlers. | Each layer tested at its level. | Repository logic only ever exercised through mocks. | Ch12 [401–448] |

## 12. Integration and anticorruption  `VN-INT`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| VN-INT-1 | Assume the network fails: prefer asynchronous messaging over synchronous calls for autonomy; when calling synchronously, isolate failures. | Remote failures otherwise fail the caller. | — | Ch13 [449–508] |
| VN-INT-2 | ACL shape: a domain-level interface stated in our terms; infrastructure implementation = adapter (calls the remote side, maps status codes) + translator (builds **local** value objects). | Foreign models stay out. | PSP response objects or status strings passed into domain methods; translation code inside handlers. | Ch3 [85–110], Ch13 [449–508] |
| VN-INT-3 | Consume only the fields you need from foreign messages/representations; don't share the producer's classes. | Sharing classes couples contexts. | Referencing another context's domain types to deserialise its events. | Ch13 [449–508] |
| VN-INT-4 | Copy as little foreign data as possible; identities are safe to copy. Copied data makes you responsible for out-of-order/duplicate updates (track last-applied timestamps). | Sync cost grows with every copied field. | Local entities mirroring many fields of an external record. | Ch13 [449–508] |
| VN-INT-5 | Model unavailability/pending remote state explicitly as domain state (Requested, Ready, Failed). | Eventual consistency is a valid state, not a hack. | Booleans/nulls standing in for "waiting on the gateway". | Ch3 [85–110], Ch13 [449–508] |
| VN-INT-6 | Receivers are idempotent under at-least-once delivery: look up by a natural unique key before creating; unique constraints are only a backstop. | Avoids duplicates and noisy failures. | Callback handlers (e.g. PSP callbacks) that create or apply twice when called twice. | Ch13 [500–503] |
| VN-INT-7 | Failed sends are retried (capped exponential back-off) or re-queued; side effects like notifications are better driven by an event than done inline. | Reliability. | Emails/notifications sent inline in a handler that then fails. | Ch13 [499–503] |

## 13. Long-running processes  `VN-PROC`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| VN-PROC-1 | A multi-step process spanning aggregates/contexts has a state tracker (often an aggregate), a process id carried by every message, completion checks and de-duplication. | No distributed transactions. | Process state scattered across flags on several entities. | Ch4 [111–168], Ch13 [494–507] |
| VN-PROC-2 | Time-outs and retries are explicit (a tracker with retry interval and limit); full time-out triggers a compensating domain command guarded by current state. | Processes must end. | Payments stuck "pending" forever with no expiry. | Ch13 [494–502] |
| VN-PROC-3 | State transitions are guarded so repeated or late messages are harmless (only move from Requested to Ready). | Idempotency. | `MarkPaid()` callable from any state. | Ch13 [494–502] |

## 14. Presenting data  `VN-UI`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| VN-UI-1 | Views may show many aggregates, but a command changes one. | Keeps writes small. | A single POST updating several aggregates. | Ch14 [512–513] |
| VN-UI-2 | DTOs/representations are shaped per use case; assemblers must not reach deep into aggregate internals. Don't add DTO layers that buy nothing. | Coupling vs accidental complexity. | Mapping code navigating `a.B.C.D`; DTOs identical to entities. | Ch14 [513–517] |
| VN-UI-3 | Load what the view needs before the transaction ends (eager fetch); don't rely on lazy loading in the presentation layer. | Lazy loads after commit fail. | Controllers touching navigation properties after the handler returned. | Ch14 [515–516] |

## 15. Event sourcing  `VN-ES` (learning notes only)

Not used in this codebase. Cite only to explain a trade-off.

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| VN-ES-1 | Event sourcing stores an aggregate as its event stream; it buys audit history and structural freedom at the cost of tooling, skills and mandatory CQRS. | Not a silver bullet. | — | App A [539–541] |
| VN-ES-2 | Concurrency is checked on append by expected version; retry by reloading and re-running, unless the behaviour has external side effects (charging a card) — then use conflict resolution. | Avoid double side effects. | Retry loops around handlers that call a PSP. | App A [554–558] |
| VN-ES-3 | Read models (projections) are disposable and rebuildable from events. | — | — | App A [570–572] |

## 16. Testing  `VN-TEST`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| VN-TEST-1 | Write a client-style test first to discover how the domain object should be used; tests use realistic data and read like domain samples. | Tests shape the API. | Tests with meaningless values and mechanical names. | Ch1 [1–42], Ch6 [217–262] |
| VN-TEST-2 | Test behaviour and outcomes (state/events/exceptions), not internal structure: Given (prior state) → When (command) → Expect (events or exception). | Robust to refactoring. | Tests asserting private fields or call counts on internals. | App A [582–583] |
| VN-TEST-3 | Test failure scenarios of services and guards, not only the happy path. | Guards are part of the model. | A constructor with five guards and one test. | Ch7 [263–284] |
| VN-TEST-4 | Value-object tests check immutability (operation leaves the original unchanged). | Immutability is a promise. | — | Ch6 [217–262] |

---

## Using this file in a review

1. Read the overrides section first.
2. Map changed files to sections: domain entity/VO → §4–6; factory/creation → §7; command handler or
   facade → §3, §9; event or event handler → §9; repository/mapping → §11; gateway/callback/integration →
   §12–13; controller/response model → §2, §14; tests → §16.
3. Report as `[VN-xxx-n] <severity> file:line`, with What / Book / Suggest. If an Evans rule already covers
   the same defect, report it once and cite both IDs.
4. Skip rules the diff doesn't touch.
