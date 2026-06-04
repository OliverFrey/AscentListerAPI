# Development

A guide for working on the API itself — building, testing, and extending it.

## Build and run

```bash
# From the repository root
dotnet build

# Run the API (from the AscentListerAPI project directory)
cd AscentListerAPI
dotnet run
```

With `ASPNETCORE_ENVIRONMENT=Development` (the default for both launch profiles), the
Scalar UI is available at `/scalar/v1`. See [configuration.md](configuration.md) for
settings and [data-model.md](data-model.md) for migration commands.

## Tests

```bash
dotnet test
```

The `AscentListerAPI.Tests` project covers three layers:

### Unit tests

- `Controllers/AscentControllerTests.cs` — exercises the controller with a substituted
  `IAscentListerService` (NSubstitute) and asserts it delegates correctly and is marked
  `[Authorize]`.
- `Services/AscentListerServiceTests.cs` — exercises `AscentListerService` with
  substituted repositories, covering the dedup logic (reuse existing location/route,
  create missing), the empty-batch case, delegation, and error handling.

### Integration tests

- `Integration/AscentApiFactory.cs` — a `WebApplicationFactory<Program>` that swaps the
  PostgreSQL `AppDbContext` for an EF Core **in-memory** database (a fresh one per
  factory, keyed by a GUID) and registers a test auth scheme.
- `Integration/TestAuthHandler.cs` — a custom authentication handler so tests can
  authenticate without a real Keycloak/JWT.
- `Integration/AscentEndpointTests.cs` — full HTTP round-trips, including a 401 for
  unauthenticated requests and a POST-then-GET that verifies persistence over HTTP.
- `Integration/AscentListerIntegrationTests.cs` — service + repository + in-memory DB,
  verifying the full graph is persisted, shared locations aren't duplicated, and
  duplicate ascent ids in a batch throw.

### Fixtures

`Integration/Fixtures/AscentFixtures.cs` provides builders for test data (e.g.
`TestCrag()`, `TestRouteOne()`, `FlashOnRouteOne()`). Reuse these when adding tests so
data setup stays consistent.

## API documentation generation

XML doc comments on controllers, the service interface, and the models are compiled into
an XML file (`<GenerateDocumentationFile>true</GenerateDocumentationFile>` in the
`.csproj`). `Microsoft.AspNetCore.OpenApi` reads these at build time, so summaries and
`<response>` descriptions appear automatically in the OpenAPI document and the Scalar UI.
`[ProducesResponseType]` attributes on the actions describe the status codes and response
schemas.

When you add or change an endpoint, add `///` summaries and the appropriate
`[ProducesResponseType]` attributes so the generated docs stay accurate.

## Adding a new endpoint

The codebase follows a consistent controller → service → repository flow. To add, say, a
"delete an ascent" endpoint:

1. **Repository** — add the method to the relevant interface and implementation under
   `Data/Repositories/` (e.g. `Task DeleteAsync(int ascentId)` on `IAscentRepository`).
2. **Service** — add it to `IAscentListerService` and implement it in
   `AscentListerService`, putting any orchestration/validation logic here.
3. **Controller** — add the action to `AscentController`, keeping it thin: delegate to
   the service and return the result. Add XML comments and `[ProducesResponseType]`.
4. **DI** — no change needed unless you introduce a new service/repository; if you do,
   register it in `Program.cs` as scoped.
5. **Tests** — add unit tests (substituted dependencies) and, where it touches HTTP or
   persistence, an integration test using `AscentApiFactory` and the fixtures.
6. **Migration** — if the model changed, run `dotnet ef migrations add <Name>` (see
   [data-model.md](data-model.md)).
