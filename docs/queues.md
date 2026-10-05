# Queues

On FreePBX 17 the queue module is **not** exposed through GraphQL. `client.Queues`
therefore uses the REST API (`/admin/api/api/rest/queues`). Your OAuth client must
have the `rest` scope.

The REST API exposes queue configuration for reading and membership for reading
and writing. Creating, updating, or deleting queue configuration is not available
through the API on FreePBX 17 and must be done in the GUI or with `fwconsole`.

## Read queues

```csharp
var queues = await client.Queues.GetAllAsync();
foreach (var queue in queues)
{
    Console.WriteLine($"{queue.Extension}: {queue.Name}");
}

// Full settings for a single queue (raw FreePBX fields are captured in
// AdditionalSettings).
var queue = await client.Queues.GetByIdAsync("1001");
```

## Read members

```csharp
var members = await client.Queues.GetMembersAsync("1001");

Console.WriteLine("Static:  " + string.Join(", ", members.Static));
Console.WriteLine("Dynamic: " + string.Join(", ", members.Dynamic));

// Members for every queue, keyed by queue number.
var all = await client.Queues.GetAllMembersAsync();
```

## Change members

```csharp
// Replace the full member list.
await client.Queues.SetMembersAsync("1001", new QueueMembers
{
    Static = ["101", "102"],
    Dynamic = ["110"]
});

// Add / remove a single member.
await client.Queues.AddMemberAsync("1001", "103");
await client.Queues.AddMemberAsync("1001", "110", dynamic: true, penalty: 5);
await client.Queues.RemoveMemberAsync("1001", "103");
```

Changing members triggers a FreePBX configuration reload.

## Models

`QueueDto`

| Property | Type | Description |
|----------|------|-------------|
| `Extension` | string | Queue number |
| `Name` | string? | Queue name |
| `Members` | `List<string>?` | Static members (single queue reads) |
| `DynamicMembers` | `List<string>?` | Dynamic members (single queue reads) |
| `AdditionalSettings` | `Dictionary<string, JsonElement>?` | All other FreePBX queue fields |

`QueueMembers`

| Property | Type | Description |
|----------|------|-------------|
| `Static` | `List<string>` | Static members |
| `Dynamic` | `List<string>` | Dynamic members |
| `Count` | int | Total members |
