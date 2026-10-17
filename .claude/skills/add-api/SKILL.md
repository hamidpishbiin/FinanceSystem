---
name: add-api
description: Scaffold the full placeholder chain for a new write API endpoint in FinanceSystem — controller action, API model, facade interface + implementation, command and command handler. Use when Hamid asks to "add an API/endpoint/action", "add an action to X controller", or types "/add-api".
---

# Add API

Create the placeholder chain for a new endpoint. Hamid fills in properties and method bodies himself. Do not implement business logic unless he asks.

## Inputs

Take these from the request. Ask, in one `AskUserQuestion` call, only for what is missing or ambiguous:

- **Controller**: an existing one, or a new one. If the controller Hamid names doesn't exist, ask whether to create it or use the closest existing one. Don't guess.
- **Action name** and **HTTP method**.
- **Inputs** and where they come from (query string, body, route, form).
- **Module**: the domain concept the endpoint belongs to (`Payments`, `Bills`, …). It's usually obvious from the controller.

Defaults when Hamid doesn't say:
- Route: `api/<module-lowercase>/<action-lowercase>`, matching the existing `[Route("api/bill")]` + `[HttpPost("create")]` style.
- Model properties: only the inputs he named.

## Files to create or edit

Read a sibling in the same module first, and copy its namespace, folder spelling and style. For example, Payments uses `Payments/Command/`, while Bills uses `Bills/Commands/`. For a brand-new module, use the plural `Commands/`.

| # | Layer | Path (under `code/`) | Content |
|---|---|---|---|
| 1 | Controller | `Host/FinanceSystem.Presentation/Controllers/<Name>Controller.cs` | `[Route]`, `[ApiController]`, primary-ctor facade injection. Action returns **`Task<IActionResult>`**: `var response = await facade.XAsync(...); return response.Status ? Ok(response) : BadRequest(response);`. Bind with `[FromQuery]`/`[FromBody]`/`[FromRoute]` as given. Take a `CancellationToken cancellationToken = default`. |
| 2 | API model | `Interface/FinanceSystem.Interface.Contracts/<Module>/Models/<Action>Model.cs` | `public class <Action>Model` with the named inputs as `required` properties. |
| 3 | Facade interface | `Interface/FinanceSystem.Interface.Contracts/<Module>/Services/I<Name>FacadeService.cs` | Must extend **`IFacadeService`**. Without it Autofac never registers the facade and the controller fails at runtime. Method `Task<JsonResponse<T>> <Action>Async(<Action>Model model, CancellationToken cancellationToken = default)`. Permission attribute: copy the one used by sibling methods in the same interface. With no sibling, use `[HasPermission(...)]` like `ITopUpFacadeService` and tell Hamid it's a guess. |
| 4 | Facade | `Interface/FinanceSystem.Interface.WriteModel/<Name>FacadeService.cs` | Primary ctor `(ICommandBus commandBus, IEventListener listener)`. Body: `throw new NotImplementedException();`. |
| 5 | Command | `Application/FinanceSystem.Application.Contracts/<Module>/<Command or Commands>/<Action>Command.cs` | Named for the operation (VN-APP-6). Inherit the module's command base if one exists (`PaymentCommand`), otherwise `ICommand`. Same properties as the model. |
| 6 | Handler | `Application/FinanceSystem.Application/<Module>/CommandHandlers/<Action>CommandHandler.cs` | Inherit the module's base handler (`PaymentCommandHandler<T>`, `BillCommandHandler<T, TEvent>`). If the module only has the event variant, ask whether Hamid wants an event before inventing one. Override `Handle` (no-event base) or `Execute` (event base) with `throw new NotImplementedException();`. |

`T` in `JsonResponse<T>` defaults to `string`. Say it's a placeholder.

No DI changes are needed. Facades are registered through the `IFacadeService` marker and handlers by assembly scan in `Infrastructure/FinanceSystem.Bootstrap/FinanceSystemModule.cs`. If either of those conventions has changed, check that file.

## House rules

- **Return `IActionResult` from controllers**, never a raw `JsonResponse<T>` and never `ActionResult<T>`, unless Hamid says otherwise.
- **No code comments and no XML doc comments.** Explain in chat.
- **Minimal first**: no extra properties, events, validators or base classes beyond what's listed.
- Read queries (GET that changes nothing) don't get a command. Ask whether it belongs in `Interface.ReadModel` as a facade query instead.

## Finish

1. Build: `dotnet build "/Users/hamid/Dev/ Projects/FinanceSystem/code/FinanceSystem.sln"`. Report only errors in files you created. Say which errors were already there.
2. Reply with a table: layer → file → what it holds. Then list the placeholders Hamid must decide: return type `T`, permission attribute, handler base/event, and query-string tokens if a secret comes in via `[FromQuery]`.
