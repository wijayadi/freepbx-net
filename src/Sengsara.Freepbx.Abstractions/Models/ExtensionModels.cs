using System.Text.Json.Serialization;

namespace Sengsara.Freepbx.Abstractions.Models;

/// <summary>
/// Represents a FreePBX Core extension (device + user pair).
/// </summary>
public class ExtensionDto
{
    /// <summary>Relay global identifier (may be empty on some FreePBX builds).</summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>Extension number.</summary>
    [JsonPropertyName("extensionId")]
    public string ExtensionId { get; set; } = string.Empty;

    /// <summary>Asterisk technology driver, for example <c>pjsip</c> or <c>sip</c>.</summary>
    [JsonPropertyName("tech")]
    public string? Tech { get; set; }

    /// <summary>Core user attached to the extension, when present.</summary>
    [JsonPropertyName("user")]
    public CoreUserDto? User { get; set; }

    /// <summary>Core device attached to the extension, when present.</summary>
    [JsonPropertyName("coreDevice")]
    public CoreDeviceDto? CoreDevice { get; set; }

    /// <summary>Convenience accessor for the caller ID name.</summary>
    [JsonIgnore]
    public string? Name => User?.Name;
}

/// <summary>
/// Request for <c>addExtension</c>.
/// </summary>
public class AddExtensionRequest
{
    /// <summary>Unique numeric extension id.</summary>
    [JsonPropertyName("extensionId")]
    public required string ExtensionId { get; set; }

    /// <summary>Technology driver. Defaults to server configuration when omitted.</summary>
    [JsonPropertyName("tech")]
    public string? Tech { get; set; }

    /// <summary>Channel name when using DAHDi.</summary>
    [JsonPropertyName("channelName")]
    public string? ChannelName { get; set; }

    /// <summary>Caller ID name.</summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>Outbound Caller ID override.</summary>
    [JsonPropertyName("outboundCid")]
    public string? OutboundCid { get; set; }

    /// <summary>Emergency Caller ID.</summary>
    [JsonPropertyName("emergencyCid")]
    public string? EmergencyCid { get; set; }

    /// <summary>Email address for voicemail, user management and fax.</summary>
    [JsonPropertyName("email")]
    public required string Email { get; set; }

    /// <summary>Enable User Management.</summary>
    [JsonPropertyName("umEnable")]
    public bool? UmEnable { get; set; }

    /// <summary>Comma separated usermanager group ids.</summary>
    [JsonPropertyName("umGroups")]
    public string? UmGroups { get; set; }

    /// <summary>Enable voicemail.</summary>
    [JsonPropertyName("vmEnable")]
    public bool? VmEnable { get; set; }

    /// <summary>Voicemail password.</summary>
    [JsonPropertyName("vmPassword")]
    public string? VmPassword { get; set; }

    /// <summary>Caller ID.</summary>
    [JsonPropertyName("callerID")]
    public string? CallerId { get; set; }

    /// <summary>User management password.</summary>
    [JsonPropertyName("umPassword")]
    public string? UmPassword { get; set; }

    /// <summary>Maximum contacts for PJSIP extensions.</summary>
    [JsonPropertyName("maxContacts")]
    public string? MaxContacts { get; set; }
}

/// <summary>
/// Request for <c>updateExtension</c>.
/// </summary>
public class UpdateExtensionRequest
{
    /// <summary>Extension number to update.</summary>
    [JsonPropertyName("extensionId")]
    public required string ExtensionId { get; set; }

    /// <summary>Technology driver.</summary>
    [JsonPropertyName("tech")]
    public string? Tech { get; set; }

    /// <summary>Channel name when using DAHDi.</summary>
    [JsonPropertyName("channelName")]
    public string? ChannelName { get; set; }

    /// <summary>Caller ID name.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>Outbound Caller ID override.</summary>
    [JsonPropertyName("outboundCid")]
    public string? OutboundCid { get; set; }

    /// <summary>Emergency Caller ID.</summary>
    [JsonPropertyName("emergencyCid")]
    public string? EmergencyCid { get; set; }

    /// <summary>Email address.</summary>
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>Enable User Management.</summary>
    [JsonPropertyName("umEnable")]
    public bool? UmEnable { get; set; }

    /// <summary>Comma separated usermanager group ids.</summary>
    [JsonPropertyName("umGroups")]
    public string? UmGroups { get; set; }

    /// <summary>Enable voicemail.</summary>
    [JsonPropertyName("vmEnable")]
    public bool? VmEnable { get; set; }

    /// <summary>Voicemail password.</summary>
    [JsonPropertyName("vmPassword")]
    public string? VmPassword { get; set; }

    /// <summary>Caller ID.</summary>
    [JsonPropertyName("callerID")]
    public string? CallerId { get; set; }

    /// <summary>Extension secret / SIP password.</summary>
    [JsonPropertyName("extPassword")]
    public string? ExtPassword { get; set; }

    /// <summary>User management password.</summary>
    [JsonPropertyName("umPassword")]
    public string? UmPassword { get; set; }

    /// <summary>Maximum contacts for PJSIP extensions.</summary>
    [JsonPropertyName("maxContacts")]
    public string? MaxContacts { get; set; }
}

/// <summary>
/// Request for <c>createRangeofExtension</c>.
/// </summary>
public class CreateExtensionRangeRequest
{
    /// <summary>First extension number.</summary>
    [JsonPropertyName("startExtension")]
    public required long StartExtension { get; set; }

    /// <summary>Caller ID name appended to each generated extension.</summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>Number of extensions to create.</summary>
    [JsonPropertyName("numberOfExtensions")]
    public required long NumberOfExtensions { get; set; }

    /// <summary>Technology driver.</summary>
    [JsonPropertyName("tech")]
    public string? Tech { get; set; }

    /// <summary>Enable User Management.</summary>
    [JsonPropertyName("umEnable")]
    public bool? UmEnable { get; set; }

    /// <summary>Outbound Caller ID override.</summary>
    [JsonPropertyName("outboundCid")]
    public string? OutboundCid { get; set; }

    /// <summary>Comma separated usermanager group ids.</summary>
    [JsonPropertyName("umGroups")]
    public string? UmGroups { get; set; }

    /// <summary>Emergency Caller ID.</summary>
    [JsonPropertyName("emergencyCid")]
    public string? EmergencyCid { get; set; }

    /// <summary>Email address.</summary>
    [JsonPropertyName("email")]
    public required string Email { get; set; }

    /// <summary>Enable voicemail.</summary>
    [JsonPropertyName("vmEnable")]
    public bool? VmEnable { get; set; }

    /// <summary>Voicemail password.</summary>
    [JsonPropertyName("vmPassword")]
    public string? VmPassword { get; set; }
}
