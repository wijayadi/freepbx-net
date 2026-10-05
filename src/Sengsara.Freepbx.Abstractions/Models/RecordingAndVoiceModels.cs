using System.Text.Json.Serialization;

namespace Sengsara.Freepbx.Abstractions.Models;

/// <summary>
/// Represents a FreePBX system recording.
/// </summary>
public class RecordingDto
{
    /// <summary>Recording identifier.</summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>Display name.</summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>Description.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>Feature code.</summary>
    [JsonPropertyName("fcode")]
    public string? FeatureCode { get; set; }

    /// <summary>Feature code password.</summary>
    [JsonPropertyName("fcode_pass")]
    public string? FeatureCodePassword { get; set; }

    /// <summary>Language.</summary>
    [JsonPropertyName("language")]
    public string? Language { get; set; }

    /// <summary>Playback files.</summary>
    [JsonPropertyName("playback")]
    public List<string>? Playback { get; set; }

    /// <summary>Available languages.</summary>
    [JsonPropertyName("languages")]
    public List<string>? Languages { get; set; }
}

/// <summary>
/// Request for <c>addRecording</c> / <c>updateRecording</c>.
/// </summary>
public class SaveRecordingRequest
{
    /// <summary>Recording identifier. Omit to create a new recording.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>Display name (required).</summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>Description.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>Feature code.</summary>
    [JsonPropertyName("fcode")]
    public string? FeatureCode { get; set; }

    /// <summary>Feature code password.</summary>
    [JsonPropertyName("fcode_pass")]
    public string? FeatureCodePassword { get; set; }

    /// <summary>Language.</summary>
    [JsonPropertyName("language")]
    public string? Language { get; set; }

    /// <summary>Playback files.</summary>
    [JsonPropertyName("playback")]
    public List<string>? Playback { get; set; }

    /// <summary>Source file to import.</summary>
    [JsonPropertyName("file")]
    public string? File { get; set; }

    /// <summary>Codec.</summary>
    [JsonPropertyName("codec")]
    public string? Codec { get; set; }

    /// <summary>Codecs.</summary>
    [JsonPropertyName("codecs")]
    public List<string>? Codecs { get; set; }

    /// <summary>Language code.</summary>
    [JsonPropertyName("lang")]
    public string? Lang { get; set; }

    /// <summary>Temporary upload id.</summary>
    [JsonPropertyName("temporary")]
    public string? Temporary { get; set; }
}

/// <summary>
/// Represents a FreePBX music on hold class.
/// </summary>
public class MusicOnHoldDto
{
    /// <summary>Relay global identifier.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>Category name, for example <c>default</c>.</summary>
    [JsonPropertyName("category")]
    public string? Category { get; set; }

    /// <summary>Type, for example <c>files</c> or <c>custom</c>.</summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>Randomize playback.</summary>
    [JsonPropertyName("random")]
    public bool? Random { get; set; }

    /// <summary>Application used as a music source.</summary>
    [JsonPropertyName("application")]
    public string? Application { get; set; }

    /// <summary>Audio format.</summary>
    [JsonPropertyName("format")]
    public string? Format { get; set; }
}

/// <summary>
/// Represents a FreePBX voicemail mailbox.
/// </summary>
public class VoiceMailDto
{
    /// <summary>Relay global identifier.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>Operation status.</summary>
    [JsonPropertyName("status")]
    public bool? Status { get; set; }

    /// <summary>Operation message.</summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    /// <summary>Voicemail context.</summary>
    [JsonPropertyName("context")]
    public string? Context { get; set; }

    /// <summary>Voicemail password.</summary>
    [JsonPropertyName("password")]
    public string? Password { get; set; }

    /// <summary>Mailbox name.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>Email address.</summary>
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>Pager email address.</summary>
    [JsonPropertyName("pager")]
    public string? Pager { get; set; }

    /// <summary>Attach recording to email.</summary>
    [JsonPropertyName("attach")]
    public string? Attach { get; set; }

    /// <summary>Say caller id.</summary>
    [JsonPropertyName("saycid")]
    public string? SayCid { get; set; }

    /// <summary>Play envelope.</summary>
    [JsonPropertyName("envelope")]
    public string? Envelope { get; set; }

    /// <summary>Delete voicemail after emailing.</summary>
    [JsonPropertyName("delete")]
    public string? Delete { get; set; }
}

/// <summary>
/// Request for <c>enableVoiceMail</c>.
/// </summary>
public class EnableVoiceMailRequest
{
    /// <summary>Extension number.</summary>
    [JsonPropertyName("extensionId")]
    public required string ExtensionId { get; set; }

    /// <summary>Voicemail password.</summary>
    [JsonPropertyName("password")]
    public required string Password { get; set; }

    /// <summary>Mailbox name.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>Email address.</summary>
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>Pager email address.</summary>
    [JsonPropertyName("pager")]
    public string? Pager { get; set; }

    /// <summary>Say caller id.</summary>
    [JsonPropertyName("saycid")]
    public bool? SayCid { get; set; }

    /// <summary>Play envelope.</summary>
    [JsonPropertyName("envelope")]
    public bool? Envelope { get; set; }

    /// <summary>Attach recording to email.</summary>
    [JsonPropertyName("attach")]
    public bool? Attach { get; set; }

    /// <summary>Delete voicemail after emailing.</summary>
    [JsonPropertyName("delete")]
    public bool? Delete { get; set; }
}

/// <summary>
/// Represents a FreePBX follow me configuration.
/// </summary>
public class FollowMeDto
{
    /// <summary>Relay global identifier.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>Operation status.</summary>
    [JsonPropertyName("status")]
    public bool? Status { get; set; }

    /// <summary>Operation message.</summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    /// <summary>Whether follow me is enabled.</summary>
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }

    /// <summary>Extension number.</summary>
    [JsonPropertyName("extensionId")]
    public string? ExtensionId { get; set; }

    /// <summary>Ring strategy.</summary>
    [JsonPropertyName("strategy")]
    public string? Strategy { get; set; }

    /// <summary>Ring time in seconds.</summary>
    [JsonPropertyName("ringTime")]
    public int? RingTime { get; set; }

    /// <summary>Follow me prefix.</summary>
    [JsonPropertyName("followMePrefix")]
    public string? FollowMePrefix { get; set; }

    /// <summary>Follow me number list.</summary>
    [JsonPropertyName("followMeList")]
    public string? FollowMeList { get; set; }

    /// <summary>Caller message recording id.</summary>
    [JsonPropertyName("callerMessage")]
    public string? CallerMessage { get; set; }

    /// <summary>No answer destination.</summary>
    [JsonPropertyName("noAnswerDestination")]
    public string? NoAnswerDestination { get; set; }

    /// <summary>Alert info.</summary>
    [JsonPropertyName("alertInfo")]
    public string? AlertInfo { get; set; }

    /// <summary>Confirm calls.</summary>
    [JsonPropertyName("confirmCalls")]
    public bool? ConfirmCalls { get; set; }

    /// <summary>Receiver message confirmation recording id.</summary>
    [JsonPropertyName("receiverMessageConfirmCall")]
    public string? ReceiverMessageConfirmCall { get; set; }

    /// <summary>Receiver message too late recording id.</summary>
    [JsonPropertyName("receiverMessageTooLate")]
    public string? ReceiverMessageTooLate { get; set; }

    /// <summary>Ringing music.</summary>
    [JsonPropertyName("ringingMusic")]
    public string? RingingMusic { get; set; }

    /// <summary>Initial ring time in seconds.</summary>
    [JsonPropertyName("initialRingTime")]
    public int? InitialRingTime { get; set; }

    /// <summary>Voicemail context.</summary>
    [JsonPropertyName("voicemail")]
    public string? VoiceMail { get; set; }

    /// <summary>Enable calendar integration.</summary>
    [JsonPropertyName("enableCalendar")]
    public bool? EnableCalendar { get; set; }

    /// <summary>Match calendar.</summary>
    [JsonPropertyName("matchCalendar")]
    public bool? MatchCalendar { get; set; }

    /// <summary>Calendar id.</summary>
    [JsonPropertyName("calendar")]
    public string? Calendar { get; set; }

    /// <summary>Calendar group id.</summary>
    [JsonPropertyName("calendarGroup")]
    public string? CalendarGroup { get; set; }

    /// <summary>Override ringer volume.</summary>
    [JsonPropertyName("overrideRingerVolume")]
    public int? OverrideRingerVolume { get; set; }

    /// <summary>External caller id mode.</summary>
    [JsonPropertyName("externalCallerIdMode")]
    public string? ExternalCallerIdMode { get; set; }

    /// <summary>Fixed caller id.</summary>
    [JsonPropertyName("fixedCallerId")]
    public string? FixedCallerId { get; set; }
}

/// <summary>
/// Request for <c>updateFollowMe</c>.
/// </summary>
public class UpdateFollowMeRequest
{
    /// <summary>Extension number.</summary>
    [JsonPropertyName("extensionId")]
    public required string ExtensionId { get; set; }

    /// <summary>Whether follow me is enabled.</summary>
    [JsonPropertyName("enabled")]
    public required bool Enabled { get; set; }

    /// <summary>Ring strategy.</summary>
    [JsonPropertyName("strategy")]
    public string? Strategy { get; set; }

    /// <summary>Ring time in seconds.</summary>
    [JsonPropertyName("ringTime")]
    public int? RingTime { get; set; }

    /// <summary>Follow me prefix.</summary>
    [JsonPropertyName("followMePrefix")]
    public string? FollowMePrefix { get; set; }

    /// <summary>Follow me number list.</summary>
    [JsonPropertyName("followMeList")]
    public string? FollowMeList { get; set; }

    /// <summary>Caller message recording id.</summary>
    [JsonPropertyName("callerMessage")]
    public string? CallerMessage { get; set; }

    /// <summary>No answer destination.</summary>
    [JsonPropertyName("noAnswerDestination")]
    public string? NoAnswerDestination { get; set; }

    /// <summary>Alert info.</summary>
    [JsonPropertyName("alertInfo")]
    public string? AlertInfo { get; set; }

    /// <summary>Confirm calls.</summary>
    [JsonPropertyName("confirmCalls")]
    public bool? ConfirmCalls { get; set; }

    /// <summary>Receiver message confirmation recording id.</summary>
    [JsonPropertyName("receiverMessageConfirmCall")]
    public string? ReceiverMessageConfirmCall { get; set; }

    /// <summary>Receiver message too late recording id.</summary>
    [JsonPropertyName("receiverMessageTooLate")]
    public string? ReceiverMessageTooLate { get; set; }

    /// <summary>Ringing music.</summary>
    [JsonPropertyName("ringingMusic")]
    public string? RingingMusic { get; set; }

    /// <summary>Initial ring time in seconds.</summary>
    [JsonPropertyName("initialRingTime")]
    public int? InitialRingTime { get; set; }

    /// <summary>Enable calendar integration.</summary>
    [JsonPropertyName("enableCalendar")]
    public bool? EnableCalendar { get; set; }

    /// <summary>Match calendar.</summary>
    [JsonPropertyName("matchCalendar")]
    public bool? MatchCalendar { get; set; }

    /// <summary>Calendar id.</summary>
    [JsonPropertyName("calendar")]
    public string? Calendar { get; set; }

    /// <summary>Calendar group id.</summary>
    [JsonPropertyName("calendarGroup")]
    public string? CalendarGroup { get; set; }

    /// <summary>Override ringer volume.</summary>
    [JsonPropertyName("overrideRingerVolume")]
    public int? OverrideRingerVolume { get; set; }

    /// <summary>External caller id mode.</summary>
    [JsonPropertyName("externalCallerIdMode")]
    public string? ExternalCallerIdMode { get; set; }

    /// <summary>Fixed caller id.</summary>
    [JsonPropertyName("fixedCallerId")]
    public string? FixedCallerId { get; set; }
}
