# API reference

The **authoritative, always-up-to-date reference is the interactive Scalar UI**, served
in the `Development` environment at `/scalar/v1` (e.g.
http://localhost:5164/scalar/v1). It is generated from the code — XML doc comments and
`[ProducesResponseType]` attributes — so it never drifts from the implementation. The
raw OpenAPI document is at `/openapi/v1.json`.

This page is a quick summary plus copy-paste examples.

## Authentication

Every `/api/ascent` endpoint requires a JWT Bearer token from your configured provider
(see [configuration.md](configuration.md)). Send it as:

```
Authorization: Bearer <token>
```

Requests without a valid token receive `401 Unauthorized`.

A runnable version of the examples below lives in
[`AscentListerAPI.http`](../AscentListerAPI/AscentListerAPI.http) — set the `@token`
variable and send the requests straight from your IDE.

## Endpoints

### `GET /api/ascent`

Returns every recorded ascent with its route and location eagerly included.

```bash
curl http://localhost:5164/api/ascent \
  -H "Authorization: Bearer $TOKEN"
```

| Status | Meaning |
|--------|---------|
| `200` | List of ascents returned |
| `401` | Missing/invalid token |

### `POST /api/ascent/batch`

Records a batch of ascents in one request. Locations and routes that already exist (by
id) are reused; missing ones are created. Returns the persisted ascents, including any
newly created routes and locations.

```bash
curl -X POST http://localhost:5164/api/ascent/batch \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '[
    {
      "ascentId": 0,
      "date": "2026-06-04",
      "style": "Redpoint",
      "attempts": 3,
      "comments": "Crux felt easier the second day.",
      "status": "NEW",
      "route": {
        "routeId": 0,
        "routeName": "Biographie",
        "grade": "9a+",
        "gradeTwo": null,
        "routeStatus": "NEW",
        "location": {
          "locationId": 0,
          "locationName": "Céüse",
          "locationAreaName": "Hautes-Alpes",
          "locationCountry": "France",
          "locationStatus": "NEW"
        }
      }
    }
  ]'
```

| Status | Meaning |
|--------|---------|
| `200` | Batch recorded; persisted ascents returned |
| `400` | Body missing or malformed |
| `401` | Missing/invalid token |

See [data-model.md](data-model.md) for the full field reference of each entity.
