# Ring groups and inbound routes

## Ring groups

```csharp
var groups = await client.RingGroups.GetAllAsync();
var group = await client.RingGroups.GetByIdAsync("600");
```

### Create

```csharp
var created = await client.RingGroups.CreateAsync(new AddRingGroupRequest
{
    GroupNumber = "600",
    Description = "Sales ring group",
    Strategy = "ringall",
    ExtensionList = "101-102-103",
    RingTime = "20"
});
```

### Update and delete

```csharp
var updated = await client.RingGroups.UpdateAsync(new UpdateRingGroupRequest
{
    GroupNumber = "600",
    Description = "Sales team",
    ExtensionList = "101-102-103-104"
});

var result = await client.RingGroups.DeleteAsync(600);
```

### Ring strategies

`ringall`, `ringallprim`, `ringallv2`, `ringallv2prim`, `hunt`, `huntprim`,
`memoryhunt`, `memoryhuntprim`, `firstavailable`, `firstnotonphone`.

## Inbound routes (DIDs)

```csharp
var routes = await client.InboundRoutes.GetAllAsync();
var route = await client.InboundRoutes.GetByIdAsync("6285777466260/6285777466260");
```

The identifier is the route key `extension/cidnum` (as returned by FreePBX in
the `id` field).

### Create

```csharp
var created = await client.InboundRoutes.CreateAsync(new AddInboundRouteRequest
{
    Extension = "6285777466260",
    CidNum = "6285777466260",
    Description = "Main DID",
    Destination = "ext-local,101,1"
});
```

### Update and delete

```csharp
var updated = await client.InboundRoutes.UpdateAsync(new UpdateInboundRouteRequest
{
    Extension = "6285777466260",
    Destination = "ringgroups,600,1",
    Description = "Main DID to ring group"
});

var removed = await client.InboundRoutes.DeleteAsync(route!.Id!);
```

Destinations use the standard FreePBX destination string, for example
`ext-local,101,1`, `ringgroups,600,1`, `ivr,1,1`.
