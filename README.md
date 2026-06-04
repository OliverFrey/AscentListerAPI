# AscentListerAPI

The backend API for **AscentLister**, a personal climbing-route logbook. It lets you
record the climbing routes you've sent ("ascents") together with where you climbed
them and how.

AscentLister is designed as a **self-hosted, single-user system**: everyone runs their
own database, API, and mobile app. There is no central server and no user management —
the API is protected by a JWT issued by an authentication provider you run yourself
(e.g. Keycloak), using a client id and secret rather than per-user accounts.

This repository is the API. The project spans these repos:

- App: https://github.com/OliverFrey/AscentLister
- API: https://github.com/OliverFrey/AscentListerAPI

## Tech stack

| Area | Choice |
|------|--------|
| Runtime | .NET 10 / ASP.NET Core (controllers) |
| Persistence | PostgreSQL via Entity Framework Core 10 (Npgsql) |
| Auth | JWT Bearer tokens (OpenID Connect, e.g. Keycloak) |
| API docs | OpenAPI + [Scalar](https://github.com/scalar/scalar) interactive UI |
| Tests | xUnit, NSubstitute, EF Core In-Memory, `WebApplicationFactory` |

## Quick start

Prerequisites: the [.NET 10 SDK](https://dotnet.microsoft.com/download), a PostgreSQL
database, and an OpenID Connect provider (e.g. Keycloak).

```bash
# 1. Clone
git clone https://github.com/OliverFrey/AscentListerAPI.git
cd AscentListerAPI/AscentListerAPI

# 2. Configure: copy the template and fill in your DB + JWT settings
cp appsettings.json_template appsettings.json
#   (see docs/configuration.md for what each setting means)

# 3. Create the database schema from the EF Core migrations
dotnet ef database update

# 4. Run
dotnet run
```

In the `Development` environment the interactive Scalar API docs are served at
`/scalar/v1` (e.g. http://localhost:5164/scalar/v1). A plain `GET /` returns
`"API is running"` as a health check.

> **Note:** `dotnet ef` requires the EF Core CLI tools. Install once with
> `dotnet tool install --global dotnet-ef`.

## Project layout

| Path | Contents |
|------|----------|
| `Controllers/` | HTTP endpoints (`AscentController`) |
| `Services/` | Application logic (`AscentListerService`) |
| `Data/` | `AppDbContext` and the repositories |
| `Models/` | Domain entities: `Location`, `Route`, `Ascent`, `StatusEnum` |
| `Migrations/` | EF Core schema migrations |
| `Program.cs` | Startup, DI, auth, and OpenAPI/Scalar wiring |
| `../AscentListerAPI.Tests/` | Unit and integration tests |

## Documentation

- [Architecture](docs/architecture.md) — how a request flows through the layers
- [Data model](docs/data-model.md) — entities, relationships, and migrations
- [Configuration](docs/configuration.md) — connection string, JWT/Keycloak settings
- [Development](docs/development.md) — building, testing, and adding endpoints
- [API reference](docs/api.md) — endpoints and example requests

## API at a glance

All endpoints live under `/api/ascent` and require a Bearer token.

| Method | Route | Purpose |
|--------|-------|---------|
| `GET` | `/api/ascent` | List all ascents (with route + location) |
| `POST` | `/api/ascent/batch` | Record a batch of ascents |

See [docs/api.md](docs/api.md) for details and examples.
