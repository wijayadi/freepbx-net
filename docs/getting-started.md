# Getting started

## Install

```bash
dotnet add package Sengsara.Freepbx
```

## Prepare FreePBX

1. Install and enable the **API** module.
2. Open **Connectivity → API → Applications** and create an OAuth2 application.
   For server-to-server integrations choose the **Client Credentials** grant and
   note the client id and secret.
3. Grant the application the `gql` and `rest` scopes if you need both GraphQL and
   the queue REST endpoints. The `gql` scope alone is enough for GraphQL.

## Create a client

```csharp
using Sengsara.Freepbx;
using Sengsara.Freepbx.Client;

var options = new FreepbxClientOptions("https://pbx.example.com")
{
    ClientId = "your-client-id",
    ClientSecret = "your-client-secret",
    Scope = "gql rest",
    AllowInsecureCertificates = false // set true only for self-signed certs
};

using var client = new FreepbxClient(options);
```

`FreepbxClientOptions` builds the endpoint URIs from `BaseUrl` and the default
FreePBX paths:

| Property | Default |
|----------|---------|
| `GraphQLPath` | `/admin/api/api/gql` |
| `RestPath` | `/admin/api/api/rest` |
| `TokenPath` | `/admin/api/api/token` |

## Verify connectivity

```csharp
if (await client.TestConnectionAsync())
{
    Console.WriteLine("Connected!");
}
```

## Query objects

```csharp
var extensions = await client.Extensions.GetAllAsync();
var users = await client.CoreUsers.GetAllAsync();
var devices = await client.CoreDevices.GetAllAsync();
var ringGroups = await client.RingGroups.GetAllAsync();
var routes = await client.InboundRoutes.GetAllAsync();
var recordings = await client.Recordings.GetAllAsync();
var moh = await client.MusicOnHold.GetAllAsync();
var cdrs = await client.Cdrs.GetAsync(first: 50);
```

## Low level access

Every service is a thin layer over `client.GraphQL`, which you can use for
operations that are not wrapped by a service:

```csharp
var data = await client.GraphQL.ExecuteRawAsync("{ fetchAllValidExtensions { totalCount } }");
Console.WriteLine(data.RootElement.GetProperty("fetchAllValidExtensions")
    .GetProperty("totalCount").GetInt32());
```
