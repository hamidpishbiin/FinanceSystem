# Review rules — Evans, *Domain-Driven Design* (2003)

Source: Eric Evans, *Domain-Driven Design: Tackling Complexity in the Heart of Software*,
`/Users/hamid/Dev/Books/Domain-Driven Design.pdf`. Page numbers in `[brackets]` are the book's printed
page numbers (not PDF page indices).

**Role of this book in reviews:** core concepts and the reasoning behind them. When a finding needs
"why does DDD care about this?", cite Evans. Practical .NET/implementation detail (aggregate sizing,
domain events mechanics, application services) belongs to Vernon's *Implementing Domain-Driven Design*
once it is added.

These rules are paraphrased, not quoted. Each has an ID so findings can cite it, e.g. `EV-AGG-3`.

**Severity guide**

- **Breaks** — violates a rule the book treats as structural (aggregate boundary, layer direction,
  invariant enforcement). Report as a finding.
- **Smell** — the book names this as a warning sign, but context may justify it. Report with the
  trade-off.
- **Learning note** — not a defect; an opportunity to apply a concept. Mention only when relevant.

---

## Project decisions that override the book

Recorded decisions win over these rules. Flag the tension once if it matters, never as a defect.

- **Repository interfaces live in Domain** (`IXxxRepository` next to the aggregate). Consistent with
  Evans; do not suggest moving them.
- **Product types are hands-off** (`Domain/.../Products/**`, `Domain.Contract/Products/**`,
  `ProductUnitTests/**`). Report Product issues, never edit them.
- **Minimal first** — build the smallest model that meets today's need. This matches Evans' own
  minimalism (EV-PROC-4); don't recommend anticipatory structure.
- **PSP gateway settings live in `PspOptions` config**, not on the `PaymentServiceProvider` entity.
  Settled.
- **No code comments / no XML doc comments.** Never suggest adding them as a fix; explanations go in
  chat or in the distillation-style docs.
- **`EventHandlerAction<T>` result-capture in facades** is a deliberate way to return values from
  void commands. Not an EV-LAY violation; it lives in the Interface layer.
- **No database yet** — schema/migration concerns from EV-REPO-8 are advisory only.

---

## 1. Ubiquitous language  `EV-UL`

| ID | Rule | Why | Signs of violation in this codebase | Ref |
|---|---|---|---|---|
| EV-UL-1 | Class, method, property, module and event names come from the language the domain experts use. | Translation between expert terms and code terms loses knowledge and hides model splits. | Developer-invented names experts wouldn't recognise (`Manager`, `Helper`, `Data`, `Info`, `Processor`); a term used in chat/requirements that appears nowhere in code. | [24–27] |
| EV-UL-2 | A change in the language is a change in the model — rename code when the meaning changes. | Stale names mislead the next reader into the old model. | A field renamed in conversation but not in code; `ExternalServiceId` kept after the concept became `ServiceId`. | [26–27] |
| EV-UL-3 | The same term must mean one thing within a context (no false cognates), and one concept must have one name (no duplicates). | Two meanings under one name corrupt data; two names for one concept drift apart. | `InvalidServiceIdException` existing in two modules with different meanings and being aliased; `Amount` vs `Value` vs `Money` used interchangeably for the same thing. | [339–340] |
| EV-UL-4 | A term the business does not use is a warning sign, especially if it carries logic. | Evans' "Loan Investment" was a developer invention that hid the real concept. | A computed concept with no business name; a model element experts say they don't understand. | [199] |
| EV-UL-5 | Tests should read in the same language. | Tests are executable statements of the model. | Test names describing mechanics ("sets flag to true") instead of domain outcomes. | [247–249] |

## 2. Layers and isolation  `EV-LAY`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| EV-LAY-1 | Business rules live in the domain layer only. Application/command handlers coordinate: load, call domain, save, publish. | Rules in the application layer are invisible to the model and get duplicated per use case. | A command handler computing eligibility, limits, state transitions, amounts, or choosing which ledger/account — instead of asking an entity, VO, domain service or specification. | [70–71], [184–185] |
| EV-LAY-2 | The application layer holds no business state; it may hold task-progress state. | Business state outside the model escapes invariants. | Handlers/facades caching domain decisions across requests. | [70] |
| EV-LAY-3 | Dependencies point downward. Lower layers reach upper ones only via callbacks/events. | Upward dependencies tangle layers and make the domain depend on use cases. | `FinanceSystem.Domain` referencing Application/Interface/Infrastructure types; an entity calling a handler. | [72–73] |
| EV-LAY-4 | Infrastructure has no domain knowledge and does not initiate domain behaviour; it's offered as services behind interfaces. Application decides *when*, infrastructure knows *how*. | Keeps technical change from rippling into the model. | PSP gateway code (`FinanceSystem.Saman`, `.BehPardakht`) deciding payment state, or repositories containing business conditions in their LINQ. | [73–74] |
| EV-LAY-5 | Domain objects carry no presentation, persistence or transport concerns. | They exist to express the model. | EF/JSON attributes, `HttpContext`, DTO mapping or logging inside `FinanceSystem.Domain`. | [70], [115–116] |
| EV-LAY-6 | Use framework features selectively; don't let a framework dictate the shape of domain objects. | Framework-shaped objects (Evans' entity-bean lesson) become anemic. | Public setters or parameterless public constructors added only to satisfy EF/serialisers when a private constructor would do. | [74–75] |
| EV-LAY-7 | Don't split one conceptual object across technical tiers (data class here, logic class there). | Produces an anemic model plus "services" holding all behaviour. | An entity with only properties and a `XxxService` that holds all its rules. | [112–115] |

## 3. Entities  `EV-ENT`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| EV-ENT-1 | An entity is defined by identity and continuity, not attributes. Equality uses identity. | Mistaken identity corrupts data. | Entity equality by attribute values; two entities compared with reference equality across loads. | [89–92] |
| EV-ENT-2 | Identity must be unique, immutable once assigned, and survive persistence. The model defines what "same thing" means. | Identity is a domain decision, not a technical accident. | Public/settable `Id`; identity reassigned after creation; reconstitution generating a new Id. | [93–96], [145] |
| EV-ENT-3 | Decide deliberately whether identity comes from outside (external key) or is generated, and who generates it. | Determines constructor shape, factory responsibility and integration. | Constructor accepts an `id` for a DB-generated key, or a DB key used where an external business id exists. | [94–96], [144–145] |
| EV-ENT-4 | Keep entities spare: identity, attributes used to find/match it, and essential behaviour. Push other data into VOs or associated objects. | Bloated entities hide their identity and core role. | Entities accumulating gateway responses, display fields or unrelated configuration. | [93–94] |
| EV-ENT-5 | Business-meaningful events that "happened twice with the same values" are still two entities. | Value equality would merge them. | Deduplicating `AccountEntry` or payments by amount+date. | [92] |

## 4. Value objects  `EV-VO`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| EV-VO-1 | When only the attributes matter, model a value object: no identity, immutable, change = replace. | Values are safe to share, copy, compare and test. | Mutable `set` on a VO; a VO with an `Id`; code mutating a value in place. | [97–101] |
| EV-VO-2 | A VO's attributes form a conceptual whole (WHOLE VALUE). | Loose primitives scatter rules and lose meaning. | `decimal Amount` + separate validation everywhere instead of `Money`; street/city/zip as separate entity fields. | [99] |
| EV-VO-3 | Validation that defines the value belongs in the VO's constructor/factory. | Every instance is then valid by construction. | `Money` checks repeated at call sites; callers re-validating ranges a VO could guarantee. | [139], [144] |
| EV-VO-4 | Prefer passing immutable values (or copies) out of an entity, never mutable internals. | A mutable object leaving the entity can break its invariants. | Returning a `List<T>` field directly; exposing a mutable collection. | [100] |
| EV-VO-5 | Put complex calculations in VOs as side-effect-free functions returning new values. | Safe to combine and test; keeps entity commands trivial. | Arithmetic/proration logic inside an entity command or handler that could be `Money.Plus`, `SharePie.Prorated`. | [251–254], [288–292] |
| EV-VO-6 | If you need to look a value up globally or give it a back-reference, it's probably an unrecognised entity. | Values aren't found, they're constructed. | A repository for a VO; a VO holding a reference to its owner. | [102–103], [150] |

## 5. Aggregates  `EV-AGG`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| EV-AGG-1 | Group entities/VOs into aggregates with one root entity; the root alone has global identity and is responsible for the invariants. | Defines the unit of consistency and change. | Invariants checked in handlers instead of the root; child entities with their own repositories. | [126–129] |
| EV-AGG-2 | Outside objects hold references (or ids) only to the root. Inner objects may be handed out transiently, never kept. | The root can't protect what it doesn't control. | Public mutable child collections; another aggregate storing a reference to a child entity. | [128–129] |
| EV-AGG-3 | Only roots are fetched directly from storage; everything inside is reached through the root. | Direct queries for internals bypass invariants. | Repository methods returning inner entities; LINQ queries over child tables from the application layer. | [129], [149] |
| EV-AGG-4 | Every committed change leaves *all* invariants of the aggregate satisfied. | Partial updates create states the model forbids. | A method that changes state and expects a second call to "fix" it; invariant enforced only on some code paths. | [129] |
| EV-AGG-5 | Rules spanning aggregates are *eventually* consistent (events/batches), not enforced in one transaction. | Cross-aggregate locking causes contention and coupling. | One command mutating several aggregates to keep them in lockstep — check whether the rule truly needs atomicity. | [128] |
| EV-AGG-6 | Draw boundaries from business knowledge: what must be consistent, contention, rate of change. Copy volatile data from shared objects into the aggregate when it's a point-in-time fact. | Evans copies the part price into the PO line item. | An aggregate reading "live" data from another aggregate to compute a stored amount (fees, rates, prices) instead of snapshotting it. | [130–135] |
| EV-AGG-7 | High-frequency, low-contention records (event logs, handling events) are often their own aggregate rather than a growing collection inside another. | Avoids locking the parent on every append. | `AccountEntry`/history appended through a large parent aggregate on every operation. | [170–171], [177–179] |
| EV-AGG-8 | Delete/archive removes everything inside the boundary as one unit. | Orphans and dangling references. | Deleting a root without its children, or children without the root's consent. | [129] |
| EV-AGG-9 | Inner entities need only local identity. | Global ids for inner parts invite outside references. | Child entities with globally queried ids. | [127–128] |

## 6. Factories and creation  `EV-FAC`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| EV-FAC-1 | Creation is atomic: a constructor/factory can only produce a fully valid object or throw. | No half-built aggregates. | Objects created then "completed" by setters; `Build()` followed by `SetX()` before the object is valid. | [139], [143] |
| EV-FAC-2 | A public constructor is fine when the class is the type, construction is simple and all inputs are at hand; it must obey the same atomic/invariant rules. | Factories add indirection only when they hide something. | Over-engineered factory classes for trivial types — or complex multi-object assembly in a constructor. | [141–142] |
| EV-FAC-3 | Static factory methods (`Create`, `NewLoading`) express intent and hide concrete types/assembly. | Clients shouldn't know internal structure. | Application code assembling an aggregate's internals; `new` of inner entities outside the root. | [139–141], [175–176] |
| EV-FAC-4 | Adding inner elements to an existing aggregate goes through a factory method on the root. | The root keeps integrity during expansion. | `aggregate.Children.Add(new Child(...))` from outside. | [139] |
| EV-FAC-5 | Reconstitution is not creation: it must not assign a new id and needs a strategy for stored data that violates current invariants. | Continuity of identity; real data may predate rules. | EF needing a usable private parameterless constructor — missing one means EF tries the public constructor. Reconstitution re-running "creation-only" side effects (publishing *Created* events). | [145–146] |
| EV-FAC-6 | Choose a consistent failure style for factories. | Mixed exceptions/nulls confuse callers. | Some creators return null, others throw. Here: `Guard<TException>` is the house style. | [143] |
| EV-FAC-7 | Don't publish an event carrying an id the database hasn't assigned yet. | The event says something false. | `Publish(new XCreated(entity.Id))` inside a factory when `Id` is DB-generated (it's 0). | [145] (identity rules) |

## 7. Repositories  `EV-REPO`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| EV-REPO-1 | Repositories only for aggregate roots, and only those that actually need global access. | Communicates which objects are entry points. | Repositories for child entities or value objects; repositories created "just in case". | [151–152], [172–173] |
| EV-REPO-2 | A repository presents a collection-like, intention-revealing interface in model terms; it hides query/storage technology. | Keeps client code about the domain, not the DB. | `IQueryable` leaking out of repositories; SQL/EF types in Domain interfaces; method names about tables. | [151–152] |
| EV-REPO-3 | Business rules don't live in repository queries. Selection rules belong in a specification or domain method; the repository supplies data. | Rules inside queries get lost from the model. | `GetTopUpTargetAccountAsync` encoding which accounts are eligible in LINQ — a rule; consider a specification/domain service owning the rule. | [149–150], [229–233] |
| EV-REPO-4 | Repositories don't commit transactions; the client (unit of work / decorator) does. | Transaction scope is a use-case decision. | `SaveChanges` called inside repository methods. | [156] |
| EV-REPO-5 | Factories make new objects; repositories find existing ones. Avoid "find or create". | Blurs a distinction the domain usually cares about. | `GetOrCreate…` methods. | [157–159] |
| EV-REPO-6 | Prefer a repository query over a bidirectional association from a widely shared entity. | Shared entities shouldn't track every user. | `User`/`Customer` holding collections of all their accounts/payments. | [169–170], [454] |
| EV-REPO-7 | Developers must still understand repository performance. | Encapsulation doesn't remove cost. | Loading whole tables to filter in memory; N+1 traversals. | [154] |
| EV-REPO-8 | When the DB is the object store, keep the schema close to the model; outside processes must not write to it. | They bypass invariants and freeze the schema. | Raw SQL updates from scripts; table/column names diverging from model names. | [159–161] |

## 8. Services  `EV-SVC`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| EV-SVC-1 | A domain service is for an operation that is a real domain concept but doesn't belong to one entity/VO (e.g. funds transfer across two accounts). It is stateless and speaks in domain types. | Prevents forcing behaviour onto the wrong object. | Domain service with mutable fields; service methods taking/returning DTOs or primitives that should be domain types. | [104–106] |
| EV-SVC-2 | Don't give up on entities too early — a service that strips an entity of its natural behaviour makes the model procedural. | The most common mistake per Evans. | Service methods that only read an entity's fields and decide its state; `XxxManager`/`XxxProcessor` doers. | [104–105] |
| EV-SVC-3 | Distinguish application services (orchestration, input digestion, notifications), domain services (business rules) and infrastructure services (technical, no business meaning). | Keeps rules in the domain. | Infrastructure service deciding business outcomes; application service doing debit/credit logic. | [106–107] |
| EV-SVC-4 | Name services as activities in the ubiquitous language. | Same as any model element. | `PaymentHelper`, `CommonService`. | [105] |

## 9. Domain events and upward communication  `EV-EVT`

Evans (2003) predates the formal Domain Event pattern; these rules come from his layering and
aggregate guidance. Vernon will add the detailed mechanics.

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| EV-EVT-1 | Lower layers notify upper ones via events/observers, never by direct dependency. | Keeps layering one-way. | Domain code calling application/interface code. | [72–73], [480] |
| EV-EVT-2 | Cross-aggregate rules are satisfied asynchronously/eventually, via events or batch. | See EV-AGG-5. | Handlers synchronously chaining aggregate mutations to keep them consistent. | [128] |
| EV-EVT-3 | Events carry data that is true and meaningful at the time of publication. | Events are facts. | Event fields that are defaults/placeholders (id 0, empty strings). | — (derived) |

## 10. Modules  `EV-MOD`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| EV-MOD-1 | Modules (namespaces/folders) are named for domain concepts and tell the domain's story. | Module names enter the language. | Folders like `Entities/`, `ValueObjects/`, `Services/` grouping by pattern — "the story of what the developer was reading". | [109–111], [179–181] |
| EV-MOD-2 | Keep all code for one concept in one module. | High cohesion of concepts. | An aggregate's exceptions/enums/specs scattered across unrelated folders. | [115] |
| EV-MOD-3 | Refactor modules when understanding changes. | Old module names encode an old model. | A module named after a concept that has since been renamed or merged. | [111] |

## 11. Supple design  `EV-SUP`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| EV-SUP-1 | Intention-revealing interfaces: name types and members by effect and purpose, not mechanism. | Callers shouldn't read the implementation. | `Process()`, `Handle2()`, `DoWork()`, boolean flag parameters that switch behaviour. | [246–249] |
| EV-SUP-2 | Separate commands (change state, return no domain data, kept simple) from queries (no side effects). | Predictability and safe composition. | Methods that both mutate and return computed domain results; getters with side effects. | [250–251], [285–286] |
| EV-SUP-3 | State post-conditions and invariants; where the language can't, tests express them (arrange = preconditions, assert = post-conditions). | Makes side effects explicit. | Commands without tests for their resulting state; a surprising post-condition (an argument left half-changed). | [255–259] |
| EV-SUP-4 | Don't modify arguments. | A particularly risky side effect. | Methods mutating objects passed in. | [257] |
| EV-SUP-5 | Decompose along conceptual contours — the domain's natural seams — not by a fixed granularity rule. | Localised change is a sign of fit. | A small requirement forcing edits across many classes ⇒ the model needs refinement, not more patches. | [260–264] |
| EV-SUP-6 | Every dependency must earn its place; factor intricate computation into standalone classes/VOs. | Mental load grows with each dependency. | An entity depending on services, options, clocks and repositories to compute something a VO could. | [265–267] |
| EV-SUP-7 | Closure of operations: where it fits, an operation returns the same type it takes (`Money.Plus(Money)`). | High-level interfaces without new concepts. | Arithmetic on values returning primitives callers must re-wrap. | [268–270] |
| EV-SUP-8 | Don't build flexibility for imagined change; suppleness comes from alignment with the domain. | Overengineering in the name of flexibility gets in the way. | Generic plug-in points, unused interfaces, configuration nobody asked for. | [244], [264] |

## 12. Making concepts explicit: constraints, policies, specifications  `EV-EXP`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| EV-EXP-1 | A rule hidden as a guard clause or conditional in procedural code should become a named, explicit concept (method, policy object, specification). | Experts and developers can then see and discuss it. | `if` chains in handlers encoding business policy; magic numbers (`* 1.1`). | [17–20], [220–222] |
| EV-EXP-2 | Factor a constraint out of its host when it needs data the host doesn't otherwise own, is duplicated across objects, or dominates conversations while being invisible in code. | These are Evans' warning signs. | Entity injecting repositories/options only to validate one rule. | [221–222] |
| EV-EXP-3 | Use a SPECIFICATION (predicate VO, `IsSatisfiedBy`) when a Boolean business rule grows or would otherwise drift to the application layer; reuse the same spec for validation, selection and creation. | One rule, one place, three uses. | The same eligibility rule implemented once in a handler and again in a repository query. | [224–234] |
| EV-EXP-4 | Model a varying process/rule as a STRATEGY/POLICY when the domain experts talk about it as a policy. | Conditionals spread through a service otherwise. | `switch (pspCode)` / `if (type == …)` branches deciding business behaviour in several places. | [311–314] |
| EV-EXP-5 | Processes the experts talk about may be explicit (service/strategy); mechanics they don't talk about stay hidden. | Avoid modelling plumbing. | Over-modelled technical steps; or a business process buried in a batch script. | [222–223], [302–306] |
| EV-EXP-6 | Use established formalisms where they fit — **accounting** (entries never deleted, balance = sum of entries, debits = credits) and arithmetic. | Already understood by everyone; avoid reinventing. | Ledger balances updated in place without entries; corrections by editing/deleting entries instead of compensating entries; transactions whose entries don't sum to zero. | [283], [295–296] |
| EV-EXP-7 | If you use a term from a known pattern (Transaction, Accrual, Posting Rule), keep its core meaning; otherwise rename. | Borrowed terms carry expectations. | Calling something "Transaction" that doesn't move value between accounts. | [306–307] |

## 13. Bounded contexts and integration  `EV-BC`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| EV-BC-1 | Every model applies within an explicit bounded context; keep it consistent inside, translate at the edges. | Unification is only possible within a boundary. | Two features "sharing" a class whose meaning differs per feature (Evans' `Charge` story). | [331–338] |
| EV-BC-2 | Integrate external systems through an anticorruption layer: services in *our* model terms, backed by facade (their model) + adapter + translator. | Their model must not leak into ours. | PSP/bank response codes, field names or status strings flowing into domain entities unchanged; domain enums mirroring a vendor's codes. | [364–369] |
| EV-BC-3 | Name the ACL service for its responsibility in *our* domain, not after the external system. | Re-abstraction is the point. | A vendor-specific client (Saman, BehPardakht) called directly from handlers instead of through `IPspGateway`, which expresses what we need. | [182–183] |
| EV-BC-4 | Translate only what you need; don't integrate at the raw-data level. | Raw data loses the meaning that came from associations. | Passing raw dictionaries/strings from an external callback deep into the domain. | [365] |
| EV-BC-5 | Test contact points between contexts specifically. | Translation is where subtle breakage hides. | No tests around gateway translators/callback parsing. | [351] |
| EV-BC-6 | Don't share code between contexts; integration goes through translation (or an explicit shared kernel with joint tests). | Silent coupling and false cognates. | Domain types from one context referenced directly by another's model. | [344], [354–355] |
| EV-BC-7 | Consider SEPARATE WAYS before integrating: being related in a use case doesn't mean features must share a model. | Integration is always expensive. | Coupling two features that never call each other or share data. | [371–373] |
| EV-BC-8 | An internal model must not double as the public interchange format; use a deliberate published contract. | It freezes the model. | Domain entities serialised directly as API responses or messages. | [375] |

## 14. Distillation  `EV-DIST`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| EV-DIST-1 | Know the CORE DOMAIN and invest the most care there; keep it small. | Most value per unit of effort. | Elaborate design in generic areas while core rules are scattered. | [400–402] |
| EV-DIST-2 | Generic subdomains go into their own modules with no trace of core-specific concepts; implement only the part you need; don't design them for reuse. | Keeps the core visible; generic ≠ reusable. | Payment-specific concepts inside a generic money/time/id module; "generic" abstractions with one user. | [406–413] |
| EV-DIST-3 | Extract coherent mechanisms (algorithms) behind intention-revealing interfaces so the model states the "what". | "A model proposes; a mechanism disposes." | Domain classes swamped by algorithmic code (retry loops, crypto, graph traversal). | [422–424] |
| EV-DIST-4 | In pain-driven refactoring, if the root cause touches the core or the core↔support relationship, fix that first. | Best refactoring return. | Patching symptoms at the edges of a core aggregate repeatedly. | [437] |

## 15. Process and refactoring  `EV-PROC`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| EV-PROC-1 | Refactor when the design no longer expresses current understanding, a concept is implicit, or a core part could be suppler. | Waiting for full justification means waiting too long. | Known model mismatches left in place "until later". | [324–325] |
| EV-PROC-2 | Don't introduce "deeper models" the domain expert wouldn't recognise, or suppleness that is technical virtuosity. | Depth is about the domain, not cleverness. | Abstract frameworks/generic engines replacing straightforward domain code. | [325] |
| EV-PROC-3 | Symptoms of a wrong model: every new requirement complicates the design, special cases pile up, persistent numeric/rounding bugs, terms experts don't use. | Evans' breakthrough story. | Growing flag/enum branches; patch-on-patch around one concept. | [194–199] |
| EV-PROC-4 | Minimalism: apply a pattern only when it's needed; implement only the parts of it you use. | Unneeded structure gets in someone's way. | Full composite/specification frameworks for one use; layers of indirection for future needs. | [278], [320], [446], [494–495] |
| EV-PROC-5 | Consistency across the team beats locally optimal choices once a structure is agreed. | The whole stays comprehensible. | One aggregate following a different creation/validation style than its siblings without reason. | [460] |

---

## Using this file in a review

1. Read the diff; for each changed file, decide which sections apply (domain entity → §3–6, §11–12;
   command handler → §2, §8, §9; repository → §7; gateway/integration → §13).
2. Report findings as `[EV-xxx-n] <severity> file:line — what and why`, one line of book reasoning,
   one concrete suggestion. Check the overrides section first.
3. Don't report rules the diff doesn't touch. Prefer three meaningful findings over thirty nitpicks.
