# Extensions

Extensions represent individual phone lines or devices in FreePBX.

## Getting Extensions

### Get All Extensions

```csharp
var extensions = await client.Extensions.GetAllAsync();
```

### Get Extension by ID

```csharp
var extension = await client.Extensions.GetByIdAsync("123");
```

## Creating Extensions

```csharp
var request = new CreateExtensionRequest
{
    Extension = "1001",
    Name = "John Doe",
    Email = "john@example.com",
    Department = "Sales",
    Description = "Sales team member",
    OutboundCid = 5551234567
};

var created = await client.Extensions.CreateAsync(request);
```

## Updating Extensions

```csharp
var request = new UpdateExtensionRequest
{
    Name = "John Smith",
    Email = "john.smith@example.com",
    Department = "Marketing"
};

var updated = await client.Extensions.UpdateAsync("123", request);
```

## Deleting Extensions

```csharp
var deleted = await client.Extensions.DeleteAsync("123");
```

## Extension Model

| Property | Type | Description |
|----------|------|-------------|
| Id | string | Unique identifier |
| Extension | string | Extension number |
| Name | string | Display name |
| Email | string? | Email address |
| Department | string? | Department |
| Description | string? | Description |
| OutboundCid | int? | Outbound Caller ID |
| DeviceType | string? | Type of device |
| UserLevel | string? | User permission level |
| CreateDate | DateTime? | Creation date |
| ModifyDate | DateTime? | Last modification date |
| Enabled | bool | Whether extension is enabled |