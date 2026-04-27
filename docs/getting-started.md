# Getting Started with Sengsara.Freepbx

A .NET library for interacting with FreePBX through its GraphQL API.

## Installation

Install via NuGet:

```bash
dotnet add package Sengsara.Freepbx
```

## Quick Start

### Basic Usage

```csharp
using Sengsara.Freepbx;
using Sengsara.Freepbx.Client;

// Create client options
var options = new FreepbxClientOptions("https://your-freepbx-instance.com/graphql")
{
    ApiKey = "your-api-key",
    TimeoutSeconds = 30
};

// Create the client
using var client = new FreepbxClient(options);

// Test connection
var isConnected = await client.TestConnectionAsync();

// Get extensions
var extensions = await client.Extensions.GetAllAsync();

// Get queues
var queues = await client.Queues.GetAllAsync();
```

### ASP.NET Integration

Add to your `Program.cs`:

```csharp
builder.Services.AddFreePbxFromConfiguration();
```

Then configure in `appsettings.json`:

```json
{
  "Freepbx": {
    "Endpoint": "https://your-freepbx-instance.com/graphql",
    "ApiKey": "your-api-key",
    "TimeoutSeconds": 30,
    "EnableRetry": true,
    "MaxRetryAttempts": 3
  }
}
```

### Dependency Injection

```csharp
using Microsoft.Extensions.DependencyInjection;
using Sengsara.Freepbx;

services.AddFreePbx(options =>
{
    options.Endpoint = "https://your-freepbx-instance.com/graphql";
    options.ApiKey = "your-api-key";
});
```

## Requirements

- .NET 8.0 or higher
- FreePBX with GraphQL API module installed

## License

MIT License - see [LICENSE](LICENSE) for details.