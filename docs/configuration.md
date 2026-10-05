# Configuration

## Options

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `BaseUrl` | string | - | Root URL, for example `https://pbx.example.com` |
| `GraphQLPath` | string | `/admin/api/api/gql` | GraphQL endpoint path |
| `RestPath` | string | `/admin/api/api/rest` | REST API root path |
| `TokenPath` | string | `/admin/api/api/token` | OAuth2 token endpoint path |
| `ClientId` | string? | - | OAuth2 client id |
| `ClientSecret` | string? | - | OAuth2 client secret |
| `Username` | string? | - | User Management username for the password grant |
| `Password` | string? | - | User Management password for the password grant |
| `Scope` | string | `gql rest` | Space separated OAuth2 scopes |
| `AccessToken` | string? | - | Pre-acquired bearer token |
| `AccessTokenExpiresAt` | DateTimeOffset? | - | Expiry of `AccessToken` |
| `TimeoutSeconds` | int | `30` | HTTP timeout |
| `EnableRetry` | bool | `true` | Retry transient failures |
| `MaxRetryAttempts` | int | `3` | Retry attempts |
| `AllowInsecureCertificates` | bool | `false` | Accept self-signed certificates |
| `UserAgent` | string | `Sengsara.Freepbx` | HTTP user agent |

## appsettings.json

```json
{
  "FreePbx": {
    "BaseUrl": "https://pbx.example.com",
    "GraphQLPath": "/admin/api/api/gql",
    "RestPath": "/admin/api/api/rest",
    "TokenPath": "/admin/api/api/token",
    "ClientId": "your-client-id",
    "ClientSecret": "your-client-secret",
    "Scope": "gql rest",
    "TimeoutSeconds": 30,
    "EnableRetry": true,
    "MaxRetryAttempts": 3,
    "AllowInsecureCertificates": false
  }
}
```

```csharp
builder.Services.AddFreePbxFromConfiguration(builder.Configuration);
```

## Programmatic registration

```csharp
builder.Services.AddFreePbx(options =>
{
    options.BaseUrl = builder.Configuration["FreePbx:BaseUrl"]!;
    options.ClientId = builder.Configuration["FreePbx:ClientId"];
    options.ClientSecret = builder.Configuration["FreePbx:ClientSecret"];
});
```

The following services are registered as singletons: `IFreepbxClient`,
`IExtensionService`, `ICoreUserService`, `ICoreDeviceService`, `IRingGroupService`,
`IInboundRouteService`, `IRecordingService`, `IMusicOnHoldService`,
`IVoiceMailService`, `IFollowMeService`, `ICdrService`, and `IQueueService`.

## Environment variables

The integration tests read the following environment variables (or a local,
git-ignored `freepbx.test.json` file):

| Variable | Description |
|----------|-------------|
| `FREEPBX_BASE_URL` | Base URL |
| `FREEPBX_CLIENT_ID` | OAuth2 client id |
| `FREEPBX_CLIENT_SECRET` | OAuth2 client secret |
| `FREEPBX_USERNAME` / `FREEPBX_PASSWORD` | Password grant credentials |
| `FREEPBX_ACCESS_TOKEN` | Static token |
| `FREEPBX_ALLOW_INSECURE` | `true` to accept self-signed certificates |
| `FREEPBX_ENABLE_WRITE_TESTS` | `true` to run create/delete integration tests |
