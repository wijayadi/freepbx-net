using System.Text.Json.Serialization;

namespace Sengsara.Freepbx.Abstractions.Models;

/// <summary>
/// Represents a FreePBX ring group.
/// </summary>
public class RingGroupDto
{
    /// <summary>Relay global identifier.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>Group number.</summary>
    [JsonPropertyName("groupNumber")]
    public int? GroupNumber { get; set; }

    /// <summary>Description shown in the directory.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>Comma separated list of members.</summary>
    [JsonPropertyName("groupList")]
    public string? GroupList { get; set; }

    /// <summary>Ring time in seconds.</summary>
    [JsonPropertyName("groupTime")]
    public int? GroupTime { get; set; }

    /// <summary>Prefix added to the caller ID.</summary>
    [JsonPropertyName("groupPrefix")]
    public string? GroupPrefix { get; set; }

    /// <summary>Require confirmation before connecting.</summary>
    [JsonPropertyName("needConf")]
    public bool? NeedConf { get; set; }

    /// <summary>Override ringer volume.</summary>
    [JsonPropertyName("overrideRingerVolume")]
    public string? OverrideRingerVolume { get; set; }

    /// <summary>Caller ID mode.</summary>
    [JsonPropertyName("changecid")]
    public string? ChangeCid { get; set; }

    /// <summary>Fixed caller ID.</summary>
    [JsonPropertyName("fixedcid")]
    public string? FixedCid { get; set; }

    /// <summary>Call recording policy.</summary>
    [JsonPropertyName("callRecording")]
    public string? CallRecording { get; set; }

    /// <summary>Enable call progress indicators.</summary>
    [JsonPropertyName("pickupCall")]
    public bool? PickupCall { get; set; }

    /// <summary>Enable call progress.</summary>
    [JsonPropertyName("callProgress")]
    public bool? CallProgress { get; set; }

    /// <summary>Mark answered elsewhere.</summary>
    [JsonPropertyName("answeredElseWhere")]
    public bool? AnsweredElseWhere { get; set; }

    /// <summary>Ignore call forward.</summary>
    [JsonPropertyName("ignoreCallForward")]
    public bool? IgnoreCallForward { get; set; }

    /// <summary>Ignore call waiting.</summary>
    [JsonPropertyName("ignoreCallWait")]
    public bool? IgnoreCallWait { get; set; }

    /// <summary>Alert info.</summary>
    [JsonPropertyName("alertInfo")]
    public string? AlertInfo { get; set; }

    /// <summary>Receiver message confirmation call.</summary>
    [JsonPropertyName("receiverMessageConfirmCall")]
    public string? ReceiverMessageConfirmCall { get; set; }

    /// <summary>Receiver message.</summary>
    [JsonPropertyName("receiverMessage")]
    public string? ReceiverMessage { get; set; }

    /// <summary>Post answer action.</summary>
    [JsonPropertyName("postAnswer")]
    public string? PostAnswer { get; set; }

    /// <summary>Caller message.</summary>
    [JsonPropertyName("callerMessage")]
    public string? CallerMessage { get; set; }

    /// <summary>Ringing music.</summary>
    [JsonPropertyName("ringingMusic")]
    public string? RingingMusic { get; set; }

    /// <summary>Ring strategy.</summary>
    [JsonPropertyName("strategy")]
    public string? Strategy { get; set; }
}

/// <summary>
/// Request for <c>addRingGroup</c>.
/// </summary>
public class AddRingGroupRequest
{
    /// <summary>Group number.</summary>
    [JsonPropertyName("groupNumber")]
    public required string GroupNumber { get; set; }

    /// <summary>Description.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>Ring strategy (required), for example <c>ringall</c>.</summary>
    [JsonPropertyName("strategy")]
    public required string Strategy { get; set; }

    /// <summary>Comma separated extension list (required).</summary>
    [JsonPropertyName("extensionList")]
    public required string ExtensionList { get; set; }

    /// <summary>Ring time in seconds.</summary>
    [JsonPropertyName("ringTime")]
    public string? RingTime { get; set; }

    /// <summary>Prefix added to the caller ID.</summary>
    [JsonPropertyName("groupPrefix")]
    public string? GroupPrefix { get; set; }

    /// <summary>Caller message.</summary>
    [JsonPropertyName("callerMessage")]
    public string? CallerMessage { get; set; }

    /// <summary>Post answer action.</summary>
    [JsonPropertyName("postAnswer")]
    public string? PostAnswer { get; set; }

    /// <summary>Alert info.</summary>
    [JsonPropertyName("alertInfo")]
    public string? AlertInfo { get; set; }

    /// <summary>Require confirmation.</summary>
    [JsonPropertyName("needConf")]
    public bool? NeedConf { get; set; }

    /// <summary>Receiver message confirmation call.</summary>
    [JsonPropertyName("receiverMessageConfirmCall")]
    public string? ReceiverMessageConfirmCall { get; set; }

    /// <summary>Receiver message.</summary>
    [JsonPropertyName("receiverMessage")]
    public string? ReceiverMessage { get; set; }

    /// <summary>Ringing music.</summary>
    [JsonPropertyName("ringingMusic")]
    public string? RingingMusic { get; set; }

    /// <summary>Ignore call forward.</summary>
    [JsonPropertyName("ignoreCallForward")]
    public bool? IgnoreCallForward { get; set; }

    /// <summary>Ignore call waiting.</summary>
    [JsonPropertyName("ignoreCallWait")]
    public bool? IgnoreCallWait { get; set; }

    /// <summary>Pickup call.</summary>
    [JsonPropertyName("pickupCall")]
    public bool? PickupCall { get; set; }

    /// <summary>Call recording policy.</summary>
    [JsonPropertyName("callRecording")]
    public string? CallRecording { get; set; }

    /// <summary>Call progress.</summary>
    [JsonPropertyName("callProgress")]
    public bool? CallProgress { get; set; }

    /// <summary>Answered elsewhere.</summary>
    [JsonPropertyName("answeredElseWhere")]
    public bool? AnsweredElseWhere { get; set; }

    /// <summary>Override ringer volume.</summary>
    [JsonPropertyName("overrideRingerVolume")]
    public string? OverrideRingerVolume { get; set; }

    /// <summary>Caller ID mode.</summary>
    [JsonPropertyName("changecid")]
    public string? ChangeCid { get; set; }

    /// <summary>Fixed caller ID.</summary>
    [JsonPropertyName("fixedcid")]
    public string? FixedCid { get; set; }
}

/// <summary>
/// Request for <c>updateRingGroup</c>. Same shape as <see cref="AddRingGroupRequest"/>
/// but only <c>groupNumber</c> is required.
/// </summary>
public class UpdateRingGroupRequest
{
    /// <summary>Group number.</summary>
    [JsonPropertyName("groupNumber")]
    public required string GroupNumber { get; set; }

    /// <summary>Description.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>Ring strategy.</summary>
    [JsonPropertyName("strategy")]
    public string? Strategy { get; set; }

    /// <summary>Comma separated extension list.</summary>
    [JsonPropertyName("extensionList")]
    public string? ExtensionList { get; set; }

    /// <summary>Ring time in seconds.</summary>
    [JsonPropertyName("ringTime")]
    public string? RingTime { get; set; }

    /// <summary>Prefix added to the caller ID.</summary>
    [JsonPropertyName("groupPrefix")]
    public string? GroupPrefix { get; set; }

    /// <summary>Caller message.</summary>
    [JsonPropertyName("callerMessage")]
    public string? CallerMessage { get; set; }

    /// <summary>Post answer action.</summary>
    [JsonPropertyName("postAnswer")]
    public string? PostAnswer { get; set; }

    /// <summary>Alert info.</summary>
    [JsonPropertyName("alertInfo")]
    public string? AlertInfo { get; set; }

    /// <summary>Require confirmation.</summary>
    [JsonPropertyName("needConf")]
    public bool? NeedConf { get; set; }

    /// <summary>Receiver message confirmation call.</summary>
    [JsonPropertyName("receiverMessageConfirmCall")]
    public string? ReceiverMessageConfirmCall { get; set; }

    /// <summary>Receiver message.</summary>
    [JsonPropertyName("receiverMessage")]
    public string? ReceiverMessage { get; set; }

    /// <summary>Ringing music.</summary>
    [JsonPropertyName("ringingMusic")]
    public string? RingingMusic { get; set; }

    /// <summary>Ignore call forward.</summary>
    [JsonPropertyName("ignoreCallForward")]
    public bool? IgnoreCallForward { get; set; }

    /// <summary>Ignore call waiting.</summary>
    [JsonPropertyName("ignoreCallWait")]
    public bool? IgnoreCallWait { get; set; }

    /// <summary>Pickup call.</summary>
    [JsonPropertyName("pickupCall")]
    public bool? PickupCall { get; set; }

    /// <summary>Call recording policy.</summary>
    [JsonPropertyName("callRecording")]
    public string? CallRecording { get; set; }

    /// <summary>Call progress.</summary>
    [JsonPropertyName("callProgress")]
    public bool? CallProgress { get; set; }

    /// <summary>Answered elsewhere.</summary>
    [JsonPropertyName("answeredElseWhere")]
    public bool? AnsweredElseWhere { get; set; }

    /// <summary>Override ringer volume.</summary>
    [JsonPropertyName("overrideRingerVolume")]
    public string? OverrideRingerVolume { get; set; }

    /// <summary>Caller ID mode.</summary>
    [JsonPropertyName("changecid")]
    public string? ChangeCid { get; set; }

    /// <summary>Fixed caller ID.</summary>
    [JsonPropertyName("fixedcid")]
    public string? FixedCid { get; set; }
}
