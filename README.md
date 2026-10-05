# Sengsara.Freepbx

.NET client for the **FreePBX 17** GraphQL and REST APIs.

`Sengsara.Freepbx` provides a strongly typed, dependency-injection friendly way to
manage the standard FreePBX objects: extensions, core users, core devices, ring
groups, inbound routes (DIDs), recordings, music on hold, voicemail, follow me,
call detail records, and queues.

## Features

- OAuth2 authentication (`client_credentials`, `password`, or a static bearer token)
  with automatic token caching and refresh
- Strongly typed models for the standard FreePBX 17 objects
- Relay aware GraphQL executor with retry, logging, and rich error reporting
- REST support for queues (queues are not exposed through GraphQL on FreePBX 17)
- Dependency injection integration and `appsettings.json` binding
- Full async / `CancellationToken` support

## Requirements

- .NET 8.0 or higher
- FreePBX 17 with the `api` module installed and enabled
- An OAuth2 application registered under **Connectivity → API**

## Installation

```bash
dotnet add package Sengsara.Freepbx
```

## Quick start

```csharp
using Sengsara.Freepbx;
using Sengsara.Freepbx.Client;

var options = new FreepbxClientOptions("https://pbx.example.com")
{
    ClientId = "your-client-id",
    ClientSecret = "your-client-secret",
    Scope = "gql rest"
};

using var client = new FreepbxClient(options);

if (await client.TestConnectionAsync())
{
    foreach (var extension in await client.Extensions.GetAllAsync())
    {
        Console.WriteLine($"{extension.ExtensionId}: {extension.Name}");
    }

    foreach (var queue in await client.Queues.GetAllAsync())
    {
        Console.WriteLine($"{queue.Extension}: {queue.Name}");
    }
}
```

## Dependency injection

```csharp
builder.Services.AddFreePbxFromConfiguration(builder.Configuration);
```

```json
{
  "FreePbx": {
    "BaseUrl": "https://pbx.example.com",
    "ClientId": "your-client-id",
    "ClientSecret": "your-client-secret",
    "Scope": "gql rest"
  }
}
```

## API surface

| Service | GraphQL / REST |
|---------|----------------|
| `client.Extensions` | GraphQL (`fetchAllExtensions`, `fetchExtension`, `addExtension`, `updateExtension`, `deleteExtension`) |
| `client.CoreUsers` | GraphQL (`allCoreUsers`, `coreUser`, `addCoreUser`, `updateCoreUser`, `removeCoreUser`) |
| `client.CoreDevices` | GraphQL (`fetchAllCoreDevices`, `fetchCoreDevice`, `addCoreDevice`, `updateCoreDevice`, `deleteCoreDevice`) |
| `client.RingGroups` | GraphQL (`fetchAllRingGroups`, `fetchRingGroup`, `addRingGroup`, `updateRingGroup`, `deleteRingGroup`) |
| `client.InboundRoutes` | GraphQL (`allInboundRoutes`, `inboundRoute`, `addInboundRoute`, `updateInboundRoute`, `removeInboundRoute`) |
| `client.Recordings` | GraphQL |
| `client.MusicOnHold` | GraphQL |
| `client.VoiceMail` | GraphQL |
| `client.FollowMe` | GraphQL |
| `client.Cdrs` | GraphQL |
| `client.Queues` | REST |

## Documentation

- [Getting started](docs/getting-started.md)
- [Authentication](docs/authentication.md)
- [Configuration](docs/configuration.md)
- [Extensions](docs/extensions.md)
- [Core users and devices](docs/core-users-devices.md)
- [Ring groups and inbound routes](docs/ring-groups-and-routes.md)
- [Queues](docs/queues.md)
- [Recordings, music on hold, voicemail and follow me](docs/media-and-voicemail.md)
- [Call detail records](docs/cdrs.md)

## License

MIT License - see [LICENSE](LICENSE) for details.
