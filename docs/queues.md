# Queues

Queues manage call distribution to multiple extensions.

## Getting Queues

### Get All Queues

```csharp
var queues = await client.Queues.GetAllAsync();
```

### Get Queue by ID

```csharp
var queue = await client.Queues.GetByIdAsync("500");
```

### Get Queue Members

```csharp
var members = await client.Queues.GetMembersAsync("500");
```

## Creating Queues

```csharp
var request = new CreateQueueRequest
{
    Extension = "500",
    Description = "Sales Queue",
    Strategy = "ringall",
    MaxMembers = 10,
    Timeout = 30,
    Weight = 0,
    WrapupTime = 10,
    Autofill = true,
    MusicOnHold = "default"
};

var created = await client.Queues.CreateAsync(request);
```

## Updating Queues

```csharp
var request = new UpdateQueueRequest
{
    Description = "Premium Support Queue",
    Strategy = "leastrecent",
    MaxMembers = 5
};

var updated = await client.Queues.UpdateAsync("500", request);
```

## Deleting Queues

```csharp
var deleted = await client.Queues.DeleteAsync("500");
```

## Managing Queue Members

### Add Member

```csharp
var request = new AddQueueMemberRequest
{
    Extension = "1001",
    Penalty = "0",
    Paused = false
};

var added = await client.Queues.AddMemberAsync("500", request);
```

### Remove Member

```csharp
var removed = await client.Queues.RemoveMemberAsync("500", "1001");
```

## Queue Model

| Property | Type | Description |
|----------|------|-------------|
| Id | string | Unique identifier |
| Extension | string | Queue extension number |
| Description | string | Queue description |
| Password | string? | Queue password (if any) |
| MaxMembers | int? | Maximum members |
| Timeout | int? | Call timeout (seconds) |
| Strategy | string? | Ring strategy (ringall, roundrobin, etc.) |
| Weight | int? | Queue weight |
| WrapupTime | int? | Wrap-up time after call |
| Autofill | bool? | Auto-fill strategy |
| MusicOnHold | string? | Music on hold class |
| Enabled | bool | Whether queue is enabled |

## Ring Strategies

- `ringall` - Ring all available agents
- `leastrecent` - Ring agent with least recent call
- `fewestcalls` - Ring agent with fewest calls
- `random` - Ring random agent
- `rrmemory` - Round-robin with memory
- `linear` - Ring in order of configuration
- `wrandom` - Random with weight