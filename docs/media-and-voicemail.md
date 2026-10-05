# Recordings, music on hold, voicemail and follow me

## System recordings

```csharp
var recordings = await client.Recordings.GetAllAsync();
var files = await client.Recordings.GetFilesAsync();          // available files
var matches = await client.Recordings.GetFilesAsync("welcome");
```

### Create or update

```csharp
var result = await client.Recordings.SaveAsync(new SaveRecordingRequest
{
    Name = "Welcome message",
    Description = "Played by the main IVR",
    Playback = ["custom/welcome"]
}, create: true);
```

### Delete

```csharp
var result = await client.Recordings.DeleteAsync("1");
```

## Music on hold

Music on hold classes are read-only through this client:

```csharp
var classes = await client.MusicOnHold.GetAllAsync();
var moh = await client.MusicOnHold.GetByIdAsync("1");
```

## Voicemail

```csharp
var mailbox = await client.VoiceMail.GetAsync("101");

await client.VoiceMail.EnableAsync(new EnableVoiceMailRequest
{
    ExtensionId = "101",
    Password = "101",
    Name = "John Doe",
    Email = "john@example.com",
    Attach = true,
    SayCid = true
});

await client.VoiceMail.DisableAsync("101");
```

## Follow me

```csharp
var followMe = await client.FollowMe.GetAsync("101");

await client.FollowMe.UpdateAsync(new UpdateFollowMeRequest
{
    ExtensionId = "101",
    Enabled = true,
    RingTime = 20,
    InitialRingTime = 7,
    FollowMeList = "101",
    Strategy = "ringallv2prim"
});

await client.FollowMe.EnableAsync("101");
await client.FollowMe.DisableAsync("101");
```

The `Strategy` values are `ringallv2`, `ringallv2prim`, `ringall`, `ringallprim`,
`hunt`, `huntprim`, `memoryhunt`, `memoryhuntprim`, `firstavailable`,
`firstnotonphone`.
