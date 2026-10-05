# Core users and devices

FreePBX separates an extension into a **user** (the person/line) and a **device**
(the endpoint that registers). Both are available directly through GraphQL.

## Core users

```csharp
var users = await client.CoreUsers.GetAllAsync();

var user = await client.CoreUsers.GetByIdAsync("101");
```

### Create or update

```csharp
var created = await client.CoreUsers.CreateAsync(new AddCoreUserRequest
{
    Extension = "101",
    Name = "John Doe",
    Password = "101",
    Voicemail = "default",
    RingTimer = 20,
    CallWaiting = "enabled",
    NoAnswerDestination = "ext-local,101,dest",
    BusyDestination = "ext-local,101,busy",
    ChanUnavailDestination = "ext-local,101,unavail"
});

var updated = await client.CoreUsers.UpdateAsync(new UpdateCoreUserRequest
{
    Extension = "101",
    Name = "John Smith"
});
```

The `*_cid` and `*_dest` fields are required by the FreePBX API. The request
types default them to empty strings so you only need to set the destinations you
use.

### Remove

```csharp
var removed = await client.CoreUsers.DeleteAsync("101");
```

## Core devices

```csharp
var devices = await client.CoreDevices.GetAllAsync();
var device = await client.CoreDevices.GetByIdAsync("101");

var created = await client.CoreDevices.CreateAsync(new AddCoreDeviceRequest
{
    Id = "101",
    Tech = "pjsip",
    Dial = "PJSIP/101",
    DeviceType = "fixed",
    User = "101",
    Description = "John's desk phone"
});

var deleted = await client.CoreDevices.DeleteAsync("101");
```

### Core user model

`CoreUserDto` exposes the full FreePBX core user, including recording policies
(`RecordingInExternal`, `RecordingOutExternal`, `RecordingInInternal`,
`RecordingOutInternal`, `RecordingOnDemand`), call forwarding, do not disturb,
and caller id prefixes.

### Core device model

| Property | Type | Description |
|----------|------|-------------|
| `Id` | string? | Relay global id |
| `DeviceId` | string | Device id / extension |
| `Tech` | string | Technology driver |
| `Dial` | string | Dial string, for example `PJSIP/101` |
| `DeviceType` | string | FreePBX device type |
| `User` | `CoreUserDto?` | Attached core user |
| `Description` | string? | Description |
| `EmergencyCid` | string? | Emergency caller id |
