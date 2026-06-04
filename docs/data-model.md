# Data model

The domain has three persisted entities — `Location`, `Route`, and `Ascent` — plus a
shared `StatusEnum`. They form a simple hierarchy: a location has many routes, and a
route has many ascents.

```
Location 1 ──< Route 1 ──< Ascent
```

## Entities

### Location — `Models/Location.cs`

A climbing crag or area that routes belong to.

| Field | Type | Notes |
|-------|------|-------|
| `LocationId` | `int` | Primary key |
| `LocationName` | `string` | Required — crag/sector name |
| `LocationAreaName` | `string` | Required — broader area |
| `locationCountry` | `string` | Required — country |
| `LocationStatus` | `StatusEnum` | Required — sync lifecycle status |

> Note: `locationCountry` is camelCase while the other properties are PascalCase. This
> is an existing inconsistency in the model, kept as-is to avoid a breaking change.

### Route — `Models/Route.cs`

A climbing route at a location.

| Field | Type | Notes |
|-------|------|-------|
| `RouteId` | `int` | Primary key |
| `RouteName` | `string` | Required |
| `Grade` | `string` | Required — e.g. `"6a"`, `"9a+"` |
| `GradeTwo` | `string?` | Optional secondary grade |
| `Location` | `Location` | The owning location (FK `LocationId`) |
| `RouteStatus` | `StatusEnum` | Required — sync lifecycle status |

### Ascent — `Models/Ascent.cs`

A logged climb of a route on a given day.

| Field | Type | Notes |
|-------|------|-------|
| `AscentId` | `int` | Primary key |
| `Route` | `Route` | The route climbed (FK `RouteId`) |
| `Date` | `DateOnly` | Required — date of the ascent |
| `Style` | `string` | Required — e.g. `"Flash"`, `"Redpoint"`, `"Onsight"` |
| `Attempts` | `int` | Required — attempts to send |
| `Comments` | `string?` | Optional notes |
| `Status` | `StatusEnum` | Required — sync lifecycle status |

### StatusEnum — `Models/StatusEnum.cs`

Tracks an entity's lifecycle so the client and API can reconcile changes during sync.

| Value | Meaning |
|-------|---------|
| `NEW` | Created on the client, not yet persisted |
| `EXISTS` | Already present in the database |
| `DELETED` | Marked for deletion |

## Persistence configuration

`AppDbContext` (`Data/AppDbContext.cs`) exposes `Ascents`, `Locations`, and `Routes`
`DbSet`s. `OnModelCreating` sets primary keys, marks required columns, and configures
both relationships with `DeleteBehavior.Cascade` (deleting a location cascades to its
routes and their ascents).

## Migrations

Schema is managed with EF Core migrations under `Migrations/`:

| Migration | Change |
|-----------|--------|
| `Initial` | Creates `Locations`, `Routes`, and `Ascents` with their relationships |
| `AddSecondGradeOptionToRouteModel` | Adds the nullable `GradeTwo` column to `Routes` |
| `ChangeAscentCommentsToNullable` | Makes `Ascent.Comments` nullable |

### Working with migrations

```bash
# Apply all migrations to the configured database
dotnet ef database update

# Add a new migration after changing the model
dotnet ef migrations add <DescriptiveName>

# Roll back to a previous migration
dotnet ef database update <PreviousMigrationName>
```

Run these from the `AscentListerAPI/` project directory. They require the EF Core CLI
tools (`dotnet tool install --global dotnet-ef`).
