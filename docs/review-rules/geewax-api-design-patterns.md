# Review rules — Geewax, *API Design Patterns* (2021)

Source: JJ Geewax, *API Design Patterns* (Manning), `/Users/hamid/Dev/Books/API Design Patterns.pdf`.
References in `[brackets]` are the book's printed page numbers. **PDF page = printed page + 36**, so use
`pdftotext -f $((N+36)) -l $((M+36))` to open a range.

Chapter → printed pages: Ch1 Intro to APIs [3–13] · Ch2 Design patterns [14–30] · Ch3 Naming [33–46] ·
Ch4 Resource layout [47–64] · Ch5 Data types [65–84] · Ch6 Identifiers [87–103] · Ch7 Standard methods
[104–121] · Ch8 Partial updates [122–140] · Ch9 Custom methods [141–153] · Ch10 Long-running operations
[154–174] · Ch11 Rerunnable jobs [175–186] · Ch12 Singleton sub-resources [189–199] · Ch13 Cross references
[200–206] · Ch14 Association resources [207–217] · Ch15 Add/remove methods [218–224] · Ch16 Polymorphism
[225–236] · Ch17 Copy and move [239–253] · Ch18 Batch [254–269] · Ch19 Purge [270–277] · Ch20 Anonymous
writes [278–284] · Ch21 Pagination [285–298] · Ch22 Filtering [299–312] · Ch23 Import/export [313–332] ·
Ch24 Versioning [335–356] · Ch25 Soft deletion [357–370] · Ch26 Request deduplication [371–382] · Ch27 Request
validation [383–389] · Ch28 Revisions [390–404] · Ch29 Request retrial [405–415] · Ch30 Request
authentication [416–432].

**Role of this book in reviews:** the **API surface**: controllers, routes, HTTP verbs and status codes,
request/response models, identifiers as clients see them, errors, retries and idempotency. Evans and Vernon
cover the model behind it. When a defect is a domain problem that merely shows in the API, cite the DDD book.
Cite this one when it's about the contract.

The book is written for resource-oriented APIs in the Google AIP style (`POST /rockets/1:launch`). Use its
**principles**: predictability, no hidden side effects, safe retries, honest status codes. Don't treat its
exact URL syntax as a rule unless Hamid adopts it.

These rules are paraphrased, not quoted. Each has an ID so findings can cite it, e.g. `AP-STD-3`.

**Severity guide** (same as Evans/Vernon)

- **Breaks**: violates a rule the book treats as structural (hidden side effects, unsafe retries,
  misleading status codes, leaking resource existence). Report as a finding.
- **Smell**: the book names it as a warning sign; context may justify it. Report with the trade-off.
- **Learning note**: not a defect; an opportunity to apply a pattern. Mention only when relevant.

---

## Project decisions that override the book

Recorded decisions win over these rules. Flag the tension once if it matters, never as a defect.

- **Controllers return `IActionResult`**, mapping `JsonResponse.Status` to `Ok(...)` / `BadRequest(...)`.
  The AP status-code rules still apply *within* that shape, e.g. a not-found failure deserves 404, not 400.
- **`JsonResponse<T>` envelope** around every response. The book returns the resource itself; the envelope
  is house style.
- **No external API callers yet.** Versioning and compatibility findings (AP-VER) are advisory, and
  renames are not breaking changes today.
- **Encrypted ids are already planned** (EncryptedId + AES, pending). Don't re-report "sequential ids are
  exposed" (AP-ID-2) on every review; mention it only where a change makes it worse.
- **Minimal first.** Don't recommend the heavy patterns (long-running operations, field masks, batch,
  revisions, import/export, validate-only, rerunnable jobs) until a concrete need exists. Learning note at most.
- **No code comments / no XML doc comments.** Never suggest documenting behaviour in code.
- **Database-generated `long` ids**, as in the DDD overrides. The API-surface consequences are AP-ID notes.

**Current conventions the book disagrees with, not yet decided by Hamid.** Mention once per review as a
Learning note; don't report them as defects:
- RPC-style verb routes `api/<module>/<action>` (`api/bill/create`, `api/payment/pay`) instead of
  resource paths plus custom methods (`POST /bills`, `POST /payments/{id}:pay`).
- Enums sent as integers in request models (`int Purpose`, `int? PspCode`) instead of strings (AP-TYPE-6).
- Numeric ids in responses (`JsonResponse<long>`) instead of opaque strings (AP-ID-3).

---

## 1. What makes an API good  `AP-GOOD`

| ID | Rule | Why | Signs in this codebase | Ref |
|---|---|---|---|---|
| AP-GOOD-1 | A good API is operational (does the job), expressive (says clearly what it does), simple (no needless surface) and predictable (once learned, the rest can be guessed). | These four qualities are the yardstick for every other rule. | Two endpoints doing the same kind of thing in different shapes; an endpoint whose name hides what it does. | Ch1 [3–13] |
| AP-GOOD-2 | APIs are rigid: once clients depend on them, changing them is expensive. Get the shape right before shipping. | Patterns exist to avoid structural rewrites later. | Shipping an endpoint whose contract (types, ids, errors) is knowingly temporary. | Ch2 [14–30] |

## 2. Naming  `AP-NAME`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| AP-NAME-1 | Names must be expressive but not longer than they need to be. Every extra word must add meaning. | Too short is ambiguous; too long is noise. | `Data`, `Info`, `Model` suffixes carrying no meaning; `UserSpecifiedX`-style redundancy. | [34–36] |
| AP-NAME-2 | Same name for the same thing, different names for different things, everywhere in the API. | Clients learn a name once and predict the rest. | `ReferenceId` meaning a string in Payments and a long in Bills; `Status` vs `State` for one concept. Overlaps EV-UL-3; cite both. | [36] |
| AP-NAME-3 | Pick one language (American English), one case style and one pluralization rule, and stick to them. | Arbitrary choices must at least be consistent. | Mixed `topup` / `top-up` / `TopUp` in routes; British and American spellings mixed. | [37–40] |
| AP-NAME-4 | Methods are imperative verb + noun (`CreateBill`, `PayPayment`). Avoid prepositions (`CreateBillForUser`, `GetBillWithPayments`). A preposition usually hides a missing concept or parameter. | Prepositions multiply endpoints per variant. | Action or model names containing For/With/By/To. | [38–39], [146–147] |
| AP-NAME-5 | Primitive fields carry their unit in the name (`amountRials`, `timeoutSeconds`), or use a richer type that conveys it. | Unit confusion is silent and costly (Mars Climate Orbiter). | `Amount` with no currency or unit anywhere in the contract; durations as bare ints. | [41–45] |
| AP-NAME-6 | Avoid language-reserved words and vague pairs; use domain terms (`payer`/`payee`, not `from`/`to`). | Reserved words break generated clients; domain terms are clearer. | Fields named `from`, `to`, `type`, `object`. | [40–41] |

## 3. Resource layout  `AP-LAYOUT`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| AP-LAYOUT-1 | Store a relationship only if it gives the API useful functionality. | Every relationship costs integrity work. | Links between resources that no endpoint reads or writes. | [55–57] |
| AP-LAYOUT-2 | Make a concept its own resource when clients must interact with it on its own (atomically). Otherwise inline it as a data type. | Resources-for-everything and inline-everything are both anti-patterns. | Separate endpoints for something only ever read with its parent, or a nested blob clients must update independently. | [57–62] |
| AP-LAYOUT-3 | Avoid deep hierarchies. | Deep paths are hard to understand and manage. | Routes nesting three or more resource levels. | [60–62] |

## 4. Data types and defaults  `AP-TYPE`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| AP-TYPE-1 | Decide and document what a missing field, an explicit `null` and an empty value each mean. | Serializers disagree; clients will guess differently. | Nullable request fields with no defined meaning for null; `""` and `null` treated differently by accident. | [67–68] |
| AP-TYPE-2 | Booleans are for flags, named so that `true` is the positive case (`enableX`, not `disableX`). | Double negatives confuse. | `IsNotActive`, `DisableNotifications`. | [68–69] |
| AP-TYPE-3 | Use number types only for values you do arithmetic on. Things that merely look numeric (codes, ids, card or phone numbers) are strings. | A number type promises arithmetic meaning, range and precision. | National codes, RRNs or trace numbers typed as numbers; ids summed or compared by size. | [69–70] |
| AP-TYPE-4 | Large or decimal numbers must survive serialization exactly. When clients may parse JSON numbers as floats, send them as strings and parse with a decimal library. | Floating point and >53-bit integers silently lose precision in JavaScript and other clients. | Money amounts as JSON numbers going to a browser or JavaScript client without an agreed precision bound. | [71–73] |
| AP-TYPE-5 | Never use the zero value as a "use the default" flag. | It makes a real zero impossible to send. | `0` meaning "auto" or "not set" for amounts, priorities or codes. | [71] |
| AP-TYPE-6 | Avoid enums on the API surface; prefer string values validated on the server. If integer enums are kept, never renumber them, and decide how unknown values are handled. | New values break old clients, and integers are unreadable in logs. | `int Purpose` / `int? PspCode` in request models; renumbering an enum. | [76–77] |
| AP-TYPE-7 | Lists and maps are bounded (item count, and key/value size for maps). | Unbounded inputs are a denial-of-service risk. | Request models accepting unbounded `List<T>` or dictionaries, such as callback form fields. | [78–83] |

## 5. Identifiers  `AP-ID`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| AP-ID-1 | Identifiers are permanent and single-use: never changed, never reused after deletion. | Otherwise a stored reference means different things at different times. | Reusing ids; ids derived from mutable data. | [89] |
| AP-ID-2 | Identifiers must be unpredictable. Sequential ids let anyone enumerate and probe resources. | Turns a single misconfigured permission into a data leak. | Auto-increment `long` ids returned to clients or accepted from them. *Covered by the pending encrypted-ids plan; see overrides.* | [90] |
| AP-ID-3 | On the API surface, ids are opaque ASCII strings (the book suggests Crockford Base32 with a checksum character). Internally a UUID or long is fine; encode it at the boundary. | Strings are dense, readable and evolvable; a checksum tells "not found" apart from "mistyped". | `JsonResponse<long>` ids; numeric route ids. | [91–102] |
| AP-ID-4 | By default the server generates ids. Client-chosen ids only when there's a real need, such as offline sync, and then validated against the same format. | Clients choose poorly (collisions, guessable values). | Accepting arbitrary client ids for resources. | [96–98], [112–113] |

## 6. Standard methods  `AP-STD`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| AP-STD-1 | Standard operations (get, list, create, update, delete) behave the same way on every resource. | Predictability is the whole point. | Create returning the id here and the full object there; different not-found behaviour per controller. | [104–106] |
| AP-STD-2 | Standard methods have **no side effects**. A create stores the resource and does nothing else; calling a gateway, sending a message or moving money belongs in a custom method. | Hidden side effects make standard methods slow, partial on failure and surprising. | A "create payment" that also calls the PSP; a create that triggers external work. | [107–108], [142–144] |
| AP-STD-3 | Don't let status codes reveal whether a resource exists to someone without access. Pick the 403/404 behaviour deliberately and apply it consistently. | Otherwise attackers can probe for existing resources. | Different status codes for "not yours" and "doesn't exist" on an id-addressed endpoint. | [106] |
| AP-STD-4 | A method that isn't supported for a resource type returns 405. One that isn't allowed on this particular instance returns 403. Never fake a 404 for a missing method. | 404 says "the resource doesn't exist", which is false. | Unsupported actions answered with 404. | [106–107] |
| AP-STD-5 | Create returns the created resource, and the result is immediately readable (strong consistency: read your writes). | Clients need the server-assigned fields at once. | Returning only an id, or returning before the id exists (overlaps VN-EVT-4). | [112–114] |
| AP-STD-6 | Update modifies only the fields sent (PATCH semantics). State transitions are not updates; they are custom methods. | A state change is an action with rules and often side effects. | Clients setting `State`/`Status` fields directly. | [114], [142–143] |
| AP-STD-7 | Delete is imperative: deleting something that doesn't exist is an error (404), not a silent success. | The API reports what it did, not just the end state. | Delete endpoints returning 200 for unknown ids. | [115–116] |
| AP-STD-8 | List methods filter by what the caller may see. Avoid exact total counts (if one is needed, name it as an estimate) and server-side arbitrary sorting. Filtering is encouraged. | Counts and sorting don't scale; filtering saves traffic. | `TotalCount` on growing lists; `orderBy` parameters on large tables. | [110–112] |

## 7. Custom methods  `AP-CUSTOM`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| AP-CUSTOM-1 | Actions with side effects (pay, send, cancel, refund, reverse, verify) are custom methods: `POST`, named verb + noun, targeting the resource they act on. | That's where side effects are allowed and expected. | Side-effecting work hidden in a create or update; actions sent with GET. | [141–147] |
| AP-CUSTOM-2 | State transitions get their own custom methods (`ArchiveX`, `MarkXPaid`), not field updates. | Transitions have rules; fields don't. | "Set Status to Paid" endpoints. | [142–143] |
| AP-CUSTOM-3 | Custom methods are not a way to add parameters to a standard method (`CreateRocketForMars`). | That multiplies endpoints. | Several create endpoints differing only in one value. | [147] |
| AP-CUSTOM-4 | An action on many resources of one collection targets the collection; an action on one resource targets that resource. | Shows the scope of the action in the path. | Bulk operations attached to an unrelated parent. | [147–149] |
| AP-CUSTOM-5 | Use stateless custom methods (compute and return, store nothing) with care: state tends to become necessary later and is hard to add. | Evolvability. | Pure-computation endpoints that will clearly need configuration. | [149–151] |
| AP-CUSTOM-6 | Whatever conventions the API picks for custom methods (verb, route shape, response), it applies them consistently. | Custom methods have no built-in expectations, so consistency must come from the designer. | Each action endpoint with a different shape. | [145] |

## 8. Long-running operations  `AP-LRO`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| AP-LRO-1 | Work that is slow or depends on an external system returns an operation (a promise) whose status the client can check, instead of blocking the call. | Callers can't tell slow from hung, and can't cancel. | Endpoints that block on a gateway or bank call with no status resource. *Learning note under minimal first.* | [154–163] |
| AP-LRO-2 | An operation's result is either the resource or a structured error. A failed operation is a successful read of the operation, not an HTTP error. | Separates "the operation failed" from "reading its status failed". | — | [164–165] |
| AP-LRO-3 | Persist operations, and expire them after a stated period. | Clients come back later; storage isn't free. | — | [171–172] |

## 9. Errors  `AP-ERR`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| AP-ERR-1 | Every error carries a unique, specific, machine-readable code. Clients must never need to parse the message. | Once clients parse messages, fixing a typo breaks them. | Two failures sharing one code; failures distinguished only by message text. Supports the per-module `BusinessException.Code` ranges. | [164–165] |
| AP-ERR-2 | The HTTP status matches the failure class: 400 invalid input, 403 not allowed, 404 not found, 409 conflict, 429 rate-limited, 5xx server or downstream fault. Clients and retry logic key off it. | A blanket 400 (or 200 with `status: false`) defeats client handling and retries. | Every returned `Failure` mapped to 400 regardless of cause; failures returned with 200. | [106–107], [408–410] |

## 10. Cross references  `AP-REF`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| AP-REF-1 | A field referencing another resource is named `<resource>Id` and holds that resource's identifier. A field holding a *copy* of foreign data drops the `Id` suffix. | The name tells the type and the purpose. | `Account` holding an id; `xId` holding embedded data. | [202–203] |
| AP-REF-2 | Clients must expect references to dangle; decide deliberately between blocking the delete, clearing the reference, or tolerating it. | Each choice has costs; leave none implicit. | — | [203–204] |
| AP-REF-3 | Copying referenced data in-line means it can go stale. Do it only when a point-in-time snapshot is what the API means. | Same reasoning as EV-AGG-6 snapshots. | — | [204–206] |

## 11. Pagination and filtering  `AP-PAGE`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| AP-PAGE-1 | Large lists are paged with `maxPageSize`, an opaque `pageToken` and `nextPageToken`. Paging is finished when `nextPageToken` is empty, not when a page comes back short. | Servers may return short pages. | Lists returning whole tables; clients stopping on a short page. | [285–297] |
| AP-PAGE-2 | Avoid offset/limit paging. | Offsets are slow and skip or repeat items when data changes. | `skip`/`take` query parameters on growing collections. | [296–297] |
| AP-PAGE-3 | Filters are strings in a documented syntax, evaluated against one resource at a time. Errors in a filter are reported, not ignored. | Structured filter schemas age badly; silent errors mislead. | — | [299–312] |

## 12. Versioning and compatibility  `AP-VER`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| AP-VER-1 | Have an explicit policy for what counts as backward compatible: new fields, new resources, bug fixes that change results, semantic changes. | "Does it break existing code?" depends on who the users are. | — | [338–344] |
| AP-VER-2 | Renaming or removing fields, changing types or meaning, adding required inputs, or making previously successful calls fail are breaking changes. | Existing clients stop working. | *Advisory until there are external callers.* | [338–344] |

## 13. Safety: deduplication, validation, retries  `AP-SAFE`

| ID | Rule | Why | Signs | Ref |
|---|---|---|---|---|
| AP-SAFE-1 | Non-idempotent methods that must not run twice (payments, top-ups, transfers) accept an optional client-chosen `requestId`. The server caches the response under that id and returns the cached response on a retry. | A lost response is indistinguishable from a lost request, so clients will retry. | Payment or top-up endpoints with no idempotency key, so a client retry creates a second payment. | [371–376] |
| AP-SAFE-2 | Store a fingerprint (hash) of the request with the cached response; if a reused `requestId` arrives with a different body, return 409 Conflict. Reject malformed request ids with 400. Don't derive the id from the request body. | Clients pick colliding ids, and identical requests may legitimately repeat. | — | [375–380] |
| AP-SAFE-3 | Don't refresh cached deduplication responses when the data changes, and expire them on a short sliding window (about five minutes, refreshed on each hit). | A retry must see what the first call would have returned. | — | [376–380] |
| AP-SAFE-4 | Dangerous methods may support `validateOnly` (default false): run every check (permissions, conflicts, references), change nothing, and skip external calls that can't be dry-run. | Lets users test safely. *Learning note under minimal first.* | — | [383–389] |
| AP-SAFE-5 | Retry only what's safe to retry: 408/421/425/429/503 yes; 403/405/412/501 never; 500/502/504 only with deduplication in place. Use exponential back-off with jitter and caps. | Blind retries duplicate work or hammer a struggling server. | Outgoing PSP/bank calls retried blindly or not at all; retrying after a 504 from a gateway without an idempotency key. | [408–412] |
| AP-SAFE-6 | When the server knows when a retry will succeed (rate limits), it sends `Retry-After` in seconds, not as a date. | Durations don't depend on clock sync. | — | [412–414] |
| AP-SAFE-7 | Requests that matter must prove origin and integrity (and, where needed, prevent repudiation), e.g. by signing a fingerprint of method, path, host and body. | Unauthenticated inputs can be forged or altered. | PSP callback endpoints that accept posted fields without verifying them with the gateway or a signature. | [416–432] |
| AP-SAFE-8 | Records that must not vanish can be soft-deleted (a deleted flag or state) with undelete and expunge as custom methods. Referential rules apply as for hard delete. | Recoverability and auditability. | Hard-deleting financial records. Overlaps VN-REPO-6. | [357–370] |

---

## Using this file in a review

1. Read the overrides first, then the "current conventions" list, and mention each of those at most once
   per review.
2. Map changed files to sections: controller → §1, §6, §7, §9, §13; API/request model → §2, §4, §5, §10;
   facade → §6, §7, §9, §13; list/query endpoints → §6 (AP-STD-8), §11; gateway/callback code → §8,
   §13 (AP-SAFE-5/7); any rename of contract members → §12.
3. Report as `[AP-xxx-n] <severity> file:line`, with What / Book / Suggest. If Evans or Vernon already covers
   the same defect, report it once and cite all the IDs.
4. Skip rules the diff doesn't touch.
