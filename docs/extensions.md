# Extensions

Extensions are managed through `client.Extensions`.

## Read

```csharp
var extensions = await client.Extensions.GetAllAsync();
var valid = await client.Extensions.GetAllValidAsync();

var extension = await client.Extensions.GetByIdAsync("101");
if (extension is not null)
{
    Console.WriteLine($"{extension.ExtensionId} {extension.Name} ({extension.Tech})");
    Console.WriteLine($"Device: {extension.CoreDevice?.Dial}");
}
```

`GetByIdAsync` returns `null` when the extension does not exist.

## Create

```csharp
var result = await client.Extensions.CreateAsync(new AddExtensionRequest
{
    ExtensionId = "101",
    Name = "John Doe",
    Email = "john@example.com",
    Tech = "pjsip",
    VmEnable = true,
    VmPassword = "101",
    OutboundCid = "\"John Doe\" <5551234567>"
});

if (!result.Success)
{
    Console.WriteLine(result.Message);
}
```

FreePBX mutations return a status flag and a message, exposed through
`MutationResult`.

## Create a range

```csharp
var result = await client.Extensions.CreateRangeAsync(new CreateExtensionRangeRequest
{
    StartExtension = 200,
    NumberOfExtensions = 10,
    Name = "Agent",
    Email = "agents@example.com",
    Tech = "pjsip"
});
```

## Update

```csharp
var result = await client.Extensions.UpdateAsync(new UpdateExtensionRequest
{
    ExtensionId = "101",
    Name = "John Smith",
    Email = "john.smith@example.com",
    VmEnable = false
});
```

## Delete

```csharp
var result = await client.Extensions.DeleteAsync("101");
```

## Model

`ExtensionDto` mirrors the FreePBX `extension` type:

| Property | Type | Description |
|----------|------|-------------|
| `Id` | string? | Relay global id (may be empty on some builds) |
| `ExtensionId` | string | Extension number |
| `Tech` | string? | Technology driver (`pjsip`, `sip`, ...) |
| `User` | `CoreUserDto?` | Attached core user |
| `CoreDevice` | `CoreDeviceDto?` | Attached core device |
| `Name` | string? | Convenience accessor for `User.Name` |
