# Sengsara.Freepbx

.NET library for interacting with FreePBX through its GraphQL API.

## Overview

Sengsara.Freepbx provides a clean, type-safe .NET API for managing FreePBX extensions, queues, and agents through GraphQL. Designed for modern .NET applications with full dependency injection support.

## Features

- Full support for FreePBX GraphQL API
- Strongly-typed models for Extensions, Queues, and Agents
- Dependency injection integration for ASP.NET Core
- Comprehensive error handling
- Request retry with Polly
- Logging support
- Source generator for type-safe GraphQL queries (coming soon)

## Projects

| Project | Description |
|---------|-------------|
| `Sengsara.Freepbx.Abstractions` | Shared interfaces and models |
| `Sengsara.Freepbx` | Main client library |
| `Sengsara.Freepbx.SourceGen` | Source generator for GraphQL |
| `Sengsara.Freepbx.UnitTests` | Unit tests |
| `Sengsara.Freepbx.IntegrationTests` | Integration tests |
| `Sengsara.Freepbx.ContractTests` | Schema contract tests |

## Quick Start

```csharp
using Sengsara.Freepbx;
using Sengsara.Freepbx.Client;

var options = new FreepbxClientOptions("https://your-freepbx.com/graphql")
{
    ApiKey = "your-api-key"
};

using var client = new FreepbxClient(options);

// Get extensions
var extensions = await client.Extensions.GetAllAsync();

// Get queues  
var queues = await client.Queues.GetAllAsync();
```

## Installation

```bash
dotnet add package Sengsara.Freepbx
```

## Documentation

- [Getting Started](docs/getting-started.md)
- [Authentication](docs/authentication.md)
- [Extensions](docs/extensions.md)
- [Queues](docs/queues.md)
- [Agents](docs/agents.md)

## Requirements

- .NET 8.0 or higher
- FreePBX with GraphQL API module

## License

MIT License - see [LICENSE](LICENSE) for details.

## Contributing

Contributions are welcome! Please read our [contributing guidelines](CONTRIBUTING.md) first.