# Architecture

AscentListerAPI is a small, layered ASP.NET Core application. Each request passes
through a thin controller, into a service that holds the application logic, and down
to repositories that wrap Entity Framework Core.

```
HTTP request
    │
    ▼
AscentController          Controllers/AscentController.cs
    │  (thin: validates auth, delegates)
    ▼
IAscentListerService      Services/AscentListerService.cs
    │  (application logic: dedup, orchestration)
    ▼
Repositories              Data/Repositories/*
    │  IAscentRepository / IRouteRepository / ILocationRepository
    ▼
AppDbContext (EF Core)    Data/AppDbContext.cs
    │
    ▼
PostgreSQL
```

## Dependency injection

Everything is wired up in `Program.cs` as scoped services:

```csharp
builder.Services.AddScoped<IAscentListerService, AscentListerService>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IRouteRepository, RouteRepository>();
builder.Services.AddScoped<IAscentRepository, AscentRepository>();
```

The `AppDbContext` is registered with the PostgreSQL provider using the
`DefaultConnection` connection string. Authentication is configured as JWT Bearer
from the `Jwt` configuration section (see [configuration.md](configuration.md)).

## The two endpoints

`AscentController` exposes exactly two actions, both requiring authorization:

- `GET /api/ascent` → `service.GetAllAscentsAsync()`
- `POST /api/ascent/batch` → `service.AddAscentsAsync(...)`

## Reading: eager loading

`GetAllAscentsAsync` delegates straight to `IAscentRepository.GetAllWithGraphAsync()`,
which uses EF Core `.Include()` to eagerly load each ascent's `Route` and the route's
`Location` in one query. Clients always get the full graph, avoiding N+1 round-trips.

## Writing: batch insert with deduplication

`AscentListerService.AddAscentsAsync` is where the application logic lives. For each
incoming ascent it:

1. Looks up the ascent's `Route.Location` by id.
   - If it doesn't exist, the location is added.
   - If it exists, the incoming ascent is re-pointed at the tracked entity so EF Core
     doesn't try to insert a duplicate.
2. Repeats the same check for the `Route`.
3. After processing the whole batch, adds all ascents and saves once.

This means a batch can contain many ascents that share the same crag or route, and only
one `Location`/`Route` row is created. The whole operation is wrapped in a try/catch
that logs and rethrows on failure, so a bad batch surfaces as an error rather than a
partial write.

## Relationships and cascade deletes

Relationships are configured with the EF Core fluent API in
`AppDbContext.OnModelCreating`:

- `Location` 1 ── ∞ `Route`
- `Route` 1 ── ∞ `Ascent`

Both relationships use `DeleteBehavior.Cascade`: deleting a location deletes its routes,
which in turn deletes their ascents. See [data-model.md](data-model.md) for field-level
detail.
