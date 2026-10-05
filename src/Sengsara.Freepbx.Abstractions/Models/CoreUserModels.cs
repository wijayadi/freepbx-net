using System.Text.Json.Serialization;

namespace Sengsara.Freepbx.Abstractions.Models;

/// <summary>
/// Represents a FreePBX Core user.
/// </summary>
public class CoreUserDto
{
    /// <summary>Relay global identifier.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>Extension number.</summary>
    [JsonPropertyName("extension")]
    public string Extension { get; set; } = string.Empty;

    /// <summary>Device password.</summary>
    [JsonPropertyName("password")]
    public string? Password { get; set; }

    /// <summary>Caller ID name.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>Voicemail context, <c>novom</c> when disabled.</summary>
    [JsonPropertyName("voicemail")]
    public string? Voicemail { get; set; }

    /// <summary>Ring time in seconds before going to voicemail.</summary>
    [JsonPropertyName("ringtimer")]
    public int? RingTimer { get; set; }

    /// <summary>No answer destination.</summary>
    [JsonPropertyName("noanswer")]
    public string? NoAnswer { get; set; }

    /// <summary>Recording option.</summary>
    [JsonPropertyName("recording")]
    public string? Recording { get; set; }

    /// <summary>Outbound Caller ID.</summary>
    [JsonPropertyName("outboundCid")]
    public string? OutboundCid { get; set; }

    /// <summary>SIP name.</summary>
    [JsonPropertyName("sipname")]
    public string? SipName { get; set; }

    /// <summary>Extension secret.</summary>
    [JsonPropertyName("extPassword")]
    public string? ExtPassword { get; set; }

    /// <summary>No answer caller ID prefix.</summary>
    [JsonPropertyName("noanswerCid")]
    public string? NoAnswerCid { get; set; }

    /// <summary>Busy caller ID prefix.</summary>
    [JsonPropertyName("busyCid")]
    public string? BusyCid { get; set; }

    /// <summary>Channel unavailable caller ID prefix.</summary>
    [JsonPropertyName("chanunavailCid")]
    public string? ChanUnavailCid { get; set; }

    /// <summary>No answer destination.</summary>
    [JsonPropertyName("noanswerDestination")]
    public string? NoAnswerDestination { get; set; }

    /// <summary>Busy destination.</summary>
    [JsonPropertyName("busyDestination")]
    public string? BusyDestination { get; set; }

    /// <summary>Channel unavailable destination.</summary>
    [JsonPropertyName("chanunavailDestination")]
    public string? ChanUnavailDestination { get; set; }

    /// <summary>Music on hold class.</summary>
    [JsonPropertyName("mohclass")]
    public string? MohClass { get; set; }

    /// <summary>Call waiting enabled.</summary>
    [JsonPropertyName("callwaiting")]
    public bool? CallWaiting { get; set; }

    /// <summary>Inbound external recording policy.</summary>
    [JsonPropertyName("recording_in_external")]
    public string? RecordingInExternal { get; set; }

    /// <summary>Outbound external recording policy.</summary>
    [JsonPropertyName("recording_out_external")]
    public string? RecordingOutExternal { get; set; }

    /// <summary>Inbound internal recording policy.</summary>
    [JsonPropertyName("recording_in_internal")]
    public string? RecordingInInternal { get; set; }

    /// <summary>Outbound internal recording policy.</summary>
    [JsonPropertyName("recording_out_internal")]
    public string? RecordingOutInternal { get; set; }

    /// <summary>On demand recording policy.</summary>
    [JsonPropertyName("recording_ondemand")]
    public string? RecordingOnDemand { get; set; }

    /// <summary>Recording priority.</summary>
    [JsonPropertyName("recording_priority")]
    public int? RecordingPriority { get; set; }

    /// <summary>Unconditional call forward destination.</summary>
    [JsonPropertyName("callforward_unconditional")]
    public string? CallForwardUnconditional { get; set; }

    /// <summary>Busy call forward destination.</summary>
    [JsonPropertyName("callforward_busy")]
    public string? CallForwardBusy { get; set; }

    /// <summary>All call forward destination.</summary>
    [JsonPropertyName("callforward_all")]
    public string? CallForwardAll { get; set; }

    /// <summary>Call forward ring timer.</summary>
    [JsonPropertyName("callforward_ringtimer")]
    public int? CallForwardRingTimer { get; set; }

    /// <summary>Do not disturb enabled.</summary>
    [JsonPropertyName("donotdisturb")]
    public bool? DoNotDisturb { get; set; }
}

/// <summary>
/// Request for <c>addCoreUser</c>.
/// </summary>
public class AddCoreUserRequest
{
    /// <summary>Extension number (required).</summary>
    [JsonPropertyName("extension")]
    public required string Extension { get; set; }

    /// <summary>Device password.</summary>
    [JsonPropertyName("password")]
    public string? Password { get; set; }

    /// <summary>Caller ID name.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>Voicemail context, <c>novom</c> to disable.</summary>
    [JsonPropertyName("voicemail")]
    public string? Voicemail { get; set; }

    /// <summary>Ring time in seconds.</summary>
    [JsonPropertyName("ringtimer")]
    public int? RingTimer { get; set; }

    /// <summary>No answer destination.</summary>
    [JsonPropertyName("noanswer")]
    public string? NoAnswer { get; set; }

    /// <summary>Recording option.</summary>
    [JsonPropertyName("recording")]
    public string? Recording { get; set; }

    /// <summary>Outbound Caller ID.</summary>
    [JsonPropertyName("outboundcid")]
    public string? OutboundCid { get; set; }

    /// <summary>SIP name.</summary>
    [JsonPropertyName("sipname")]
    public string? SipName { get; set; }

    /// <summary>No answer caller ID prefix (required by the API).</summary>
    [JsonPropertyName("noanswer_cid")]
    public string NoAnswerCid { get; set; } = string.Empty;

    /// <summary>Busy caller ID prefix (required by the API).</summary>
    [JsonPropertyName("busy_cid")]
    public string BusyCid { get; set; } = string.Empty;

    /// <summary>Channel unavailable caller ID prefix (required by the API).</summary>
    [JsonPropertyName("chanunavail_cid")]
    public string ChanUnavailCid { get; set; } = string.Empty;

    /// <summary>No answer destination (required by the API).</summary>
    [JsonPropertyName("noanswer_dest")]
    public string NoAnswerDestination { get; set; } = string.Empty;

    /// <summary>Busy destination (required by the API).</summary>
    [JsonPropertyName("busy_dest")]
    public string BusyDestination { get; set; } = string.Empty;

    /// <summary>Channel unavailable destination (required by the API).</summary>
    [JsonPropertyName("chanunavail_dest")]
    public string ChanUnavailDestination { get; set; } = string.Empty;

    /// <summary>Music on hold class.</summary>
    [JsonPropertyName("mohclass")]
    public string? MohClass { get; set; }

    /// <summary>Call waiting: <c>enabled</c> or <c>disabled</c>.</summary>
    [JsonPropertyName("callwaiting")]
    public string? CallWaiting { get; set; }

    /// <summary>Inbound external recording policy.</summary>
    [JsonPropertyName("recording_in_external")]
    public string? RecordingInExternal { get; set; }

    /// <summary>Outbound external recording policy.</summary>
    [JsonPropertyName("recording_out_external")]
    public string? RecordingOutExternal { get; set; }

    /// <summary>Inbound internal recording policy.</summary>
    [JsonPropertyName("recording_in_internal")]
    public string? RecordingInInternal { get; set; }

    /// <summary>Outbound internal recording policy.</summary>
    [JsonPropertyName("recording_out_internal")]
    public string? RecordingOutInternal { get; set; }

    /// <summary>On demand recording policy.</summary>
    [JsonPropertyName("recording_ondemand")]
    public string? RecordingOnDemand { get; set; }

    /// <summary>Recording priority.</summary>
    [JsonPropertyName("recording_priority")]
    public int? RecordingPriority { get; set; }
}

/// <summary>
/// Request for <c>updateCoreUser</c>. Same shape as <see cref="AddCoreUserRequest"/>.
/// </summary>
public class UpdateCoreUserRequest : AddCoreUserRequest
{
}
