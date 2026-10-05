# Authentication

FreePBX 17 protects its API with an OAuth2 resource server. Bearer tokens are
issued by the token endpoint (`/admin/api/api/token`, by default) and are valid
for one hour.

`Sengsara.Freepbx` requests, caches, and refreshes tokens automatically.

## Client credentials (recommended for services)

```csharp
var options = new FreepbxClientOptions("https://pbx.example.com")
{
    ClientId = "your-client-id",
    ClientSecret = "your-client-secret",
    Scope = "gql rest"
};
```

This uses the `client_credentials` grant. Create the application in
**Connectivity → API** with the **Client Credentials** grant.

## Password grant

The `password` grant authenticates a FreePBX User Management account. FreePBX
requires a client id for this grant, so provide both sets of credentials:

```csharp
var options = new FreepbxClientOptions("https://pbx.example.com")
{
    ClientId = "your-client-id",
    ClientSecret = "your-client-secret",
    Username = "admin",
    Password = "your-password",
    Scope = "gql rest"
};
```

When both a client and a user are configured, the `password` grant is used.

## Static bearer token

If your application already manages OAuth tokens, pass one directly:

```csharp
var options = new FreepbxClientOptions("https://pbx.example.com")
{
    AccessToken = "eyJ...",
    AccessTokenExpiresAt = DateTimeOffset.UtcNow.AddMinutes(55) // optional
};
```

## Scopes

- `gql` - the GraphQL API (all modules)
- `rest` - the REST API (needed for queues)
- `gql rest` - both

A scope string with multiple values is space separated.

## Self-signed certificates

For development instances with self-signed TLS certificates:

```csharp
options.AllowInsecureCertificates = true;
```

> Do not enable this in production.

## Security best practices

1. Never hardcode credentials - use configuration or environment variables.
2. Grant the narrowest scope that still works for your integration.
3. Rotate client secrets regularly.
4. Always connect over HTTPS.
