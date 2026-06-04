# Configuration

Configuration lives in `appsettings.json`. The repository ships an
`appsettings.json_template` with placeholders — copy it and fill in your values:

```bash
cp appsettings.json_template appsettings.json
```

> **Keep secrets out of source control.** `appsettings.json` holds your database
> password and authentication settings. Don't commit a filled-in copy; use the template
> for sharing, and consider [user secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets)
> or environment variables for real deployments.

## Settings

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=<postgres-host>;Port=5432;Database=<database-name>;Username=<postgres-user>;Password=<postgres-password>"
  },
  "Jwt": {
    "Authority": "http://<Keycloak URL>/realms/<Realm>",
    "RequireHttpsMetadata": false,
    "ValidIssuer": "http://<Keycloak URL>/realms/<Realm>",
    "ValidAudience": "<your-api-client-id>"
  },
  "AllowedHosts": "*"
}
```

### ConnectionStrings:DefaultConnection

The PostgreSQL connection string used by `AppDbContext`. Provide host, port, database
name, and credentials for the database you created. EF Core uses this both at runtime
and for `dotnet ef database update`.

### Jwt

The API validates incoming Bearer tokens against an OpenID Connect provider (Keycloak in
the reference setup). These map directly onto the `JwtBearer` options configured in
`Program.cs`:

| Key | Meaning |
|-----|---------|
| `Authority` | The issuer/authority URL. The API fetches its signing keys from here (`{Authority}/.well-known/openid-configuration`). |
| `RequireHttpsMetadata` | Whether metadata must be fetched over HTTPS. `false` is convenient for local/LAN setups; set `true` in production. |
| `ValidIssuer` | The expected `iss` claim. Token issuer is validated against this. |
| `ValidAudience` | The expected `aud` claim — your API's client id in the provider. |

### Keycloak setup (reference)

For the reference Keycloak provider:

1. Create a realm (the `<Realm>` in the URLs above).
2. Create a client for the API; its client id becomes `ValidAudience`.
3. Set `Authority` and `ValidIssuer` to `http://<keycloak-host>/realms/<realm>`.
4. The mobile app authenticates with its client id/secret and sends the resulting access
   token as `Authorization: Bearer <token>`.

Other OpenID Connect providers work too, but the token/claim setup may differ.

## Launch URLs

Local URLs come from `Properties/launchSettings.json`:

| Profile | URL(s) | Environment |
|---------|--------|-------------|
| `http` | `http://localhost:5164` | Development |
| `https` | `https://localhost:7259`, `http://localhost:5164` | Development |

Select a profile with `dotnet run --launch-profile https`. The interactive Scalar docs
(`/scalar/v1`) and OpenAPI document are only mapped in the `Development` environment.
