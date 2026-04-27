# Agents

Agents represent users in FreePBX who can log in to queues and handle calls.

## Getting Agents

Note: Agent operations are available through the extension service as agents are closely related to extensions.

### Get All Agents

Agents can be retrieved through the extension service since each agent is associated with an extension:

```csharp
// Get all extensions (which includes agent information)
var extensions = await client.Extensions.GetAllAsync();
var agents = extensions.Where(e => !string.IsNullOrEmpty(e.Email)).ToList();
```

### Get Agent by ID

```csharp
var extension = await client.Extensions.GetByIdAsync("123");
// The extension contains agent-related information
```

## Agent Model

| Property | Type | Description |
|----------|------|-------------|
| Id | string | Unique identifier |
| Extension | string | Agent's extension |
| Username | string | Login username |
| DisplayName | string | Display name |
| Email | string? | Email address |
| FirstName | string? | First name |
| LastName | string? | Last name |
| Department | string? | Department |
| Language | string? | Preferred language |
| Timezone | string? | Timezone |
| Enabled | bool | Whether agent is enabled |
| CreateDate | DateTime? | Creation date |
| ModifyDate | DateTime? | Last modification date |

## Working with Agents

Since agents in FreePBX are essentially extensions with user accounts, you manage them through the extension service:

```csharp
// Create an extension (which creates an agent)
var request = new CreateExtensionRequest
{
    Extension = "1001",
    Name = "John Doe",
    Email = "john@example.com",
    Department = "Support"
};

var extension = await client.Extensions.CreateAsync(request);
```

## Agent Queue Status

To check an agent's status in queues:

```csharp
// Get all queues
var queues = await client.Queues.GetAllAsync();

// Check each queue for the agent
foreach (var queue in queues)
{
    var members = await client.Queues.GetMembersAsync(queue.Id);
    var agent = members.FirstOrDefault(m => m.Extension == "1001");
    
    if (agent != null)
    {
        Console.WriteLine($"Agent in queue {queue.Extension}: Status={agent.Status}, Paused={agent.Paused}");
    }
}
```

## Agent Statistics

Agent statistics are available through queue membership:

```csharp
var members = await client.Queues.GetMembersAsync("500");
var agent = members.First(m => m.Extension == "1001");

Console.WriteLine($"Calls taken: {agent.CallsTaken}");
Console.WriteLine($"Last call: {agent.LastCall}");
Console.WriteLine($"Status: {agent.Status}");
```