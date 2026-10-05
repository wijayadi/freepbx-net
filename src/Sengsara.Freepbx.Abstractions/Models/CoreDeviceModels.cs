using System.Text.Json.Serialization;

namespace Sengsara.Freepbx.Abstractions.Models;

/// <summary>
/// Represents a FreePBX Core device.
/// </summary>
public class CoreDeviceDto
{
    /// <summary>Relay global identifier.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>Device id, usually the extension number.</summary>
    [JsonPropertyName("deviceId")]
    public string DeviceId { get; set; } = string.Empty;

    /// <summary>Technology driver.</summary>
    [JsonPropertyName("tech")]
    public string Tech { get; set; } = string.Empty;

    /// <summary>Dial string, for example <c>PJSIP/101</c>.</summary>
    [JsonPropertyName("dial")]
    public string Dial { get; set; } = string.Empty;

    /// <summary>FreePBX device type, for example <c>fixed</c>.</summary>
    [JsonPropertyName("devicetype")]
    public string DeviceType { get; set; } = string.Empty;

    /// <summary>Attached core user.</summary>
    [JsonPropertyName("user")]
    public CoreUserDto? User { get; set; }

    /// <summary>Description.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>Emergency caller ID.</summary>
    [JsonPropertyName("emergencyCid")]
    public string? EmergencyCid { get; set; }
}

/// <summary>
/// Request for <c>addCoreDevice</c>.
/// </summary>
public class AddCoreDeviceRequest
{
    /// <summary>Device id / extension.</summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>Technology driver.</summary>
    [JsonPropertyName("tech")]
    public required string Tech { get; set; }

    /// <summary>Dial string.</summary>
    [JsonPropertyName("dial")]
    public required string Dial { get; set; }

    /// <summary>Device type.</summary>
    [JsonPropertyName("devicetype")]
    public required string DeviceType { get; set; }

    /// <summary>Attached user extension.</summary>
    [JsonPropertyName("user")]
    public string? User { get; set; }

    /// <summary>Description.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>Emergency caller ID.</summary>
    [JsonPropertyName("emergency_cid")]
    public string? EmergencyCid { get; set; }
}

/// <summary>
/// Request for <c>updateCoreDevice</c>. Same shape as <see cref="AddCoreDeviceRequest"/>.
/// </summary>
public class UpdateCoreDeviceRequest : AddCoreDeviceRequest
{
}
