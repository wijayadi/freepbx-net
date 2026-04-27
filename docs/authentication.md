# Authentication

Sengsara.Freepbx supports multiple authentication methods for connecting to FreePBX.

## API Key Authentication

The simplest way to authenticate is using an API key:

```csharp
var options = new FreepbxClientOptions("https://freepbx.example.com/graphql")
{
    ApiKey = "your-api-key"
};

using var client = new FreepbxClient(options);
```

## Basic Authentication

You can also use username and password:

```csharp
var options = new FreepbxClientOptions("https://freepbx.example.com/graphql")
{
    Username = "admin",
    Password = "your-password"
};

using var client = new FreepbxClient(options);
```

## Configuration File

### appsettings.json

```json
{
  "Freepbx": {
    "Endpoint": "https://freepbx.example.com/graphql",
    "ApiKey": "your-api-key",
    "Username": "admin",
    "Password": "your-password",
    "TimeoutSeconds": 30,
    "EnableRetry": true,
    "MaxRetryAttempts": 3
  }
}
```

### Environment Variables

You can also use environment variables:

```bash
export FREEPBX_ENDPOINT="https://freepbx.example.com/graphql"
export FREEPBX_API_KEY="your-api-key"
```

Then in your code:

```csharp
var options = new FreepbxClientOptions(
    Environment.GetEnvironmentVariable("FREEPBX_ENDPOINT")!)
{
    ApiKey = Environment.GetEnvironmentVariable("FREEPBX_API_KEY")
};
```

## Security Best Practices

1. **Never hardcode credentials** - Use configuration files or environment variables
2. **Use API keys when possible** - They are more secure than basic auth
3. **Rotate credentials regularly** - Update API keys periodically
4. **Use HTTPS** - Always connect over SSL/TLS