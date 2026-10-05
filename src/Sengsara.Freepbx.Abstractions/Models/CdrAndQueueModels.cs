using System.Text.Json.Serialization;

namespace Sengsara.Freepbx.Abstractions.Models;

/// <summary>
/// Represents a FreePBX call detail record.
/// </summary>
public class CdrDto
{
    /// <summary>Relay global identifier.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>Unique call id.</summary>
    [JsonPropertyName("uniqueid")]
    public string? UniqueId { get; set; }

    /// <summary>Call date/time string.</summary>
    [JsonPropertyName("calldate")]
    public string? CallDate { get; set; }

    /// <summary>Unix timestamp.</summary>
    [JsonPropertyName("timestamp")]
    public long? Timestamp { get; set; }

    /// <summary>Full caller id.</summary>
    [JsonPropertyName("clid")]
    public string? CallerId { get; set; }

    /// <summary>Source number.</summary>
    [JsonPropertyName("src")]
    public string? Source { get; set; }

    /// <summary>Destination number.</summary>
    [JsonPropertyName("dst")]
    public string? Destination { get; set; }

    /// <summary>Destination context.</summary>
    [JsonPropertyName("dcontext")]
    public string? DestinationContext { get; set; }

    /// <summary>Source channel.</summary>
    [JsonPropertyName("channel")]
    public string? Channel { get; set; }

    /// <summary>Destination channel.</summary>
    [JsonPropertyName("dstchannel")]
    public string? DestinationChannel { get; set; }

    /// <summary>Last application executed.</summary>
    [JsonPropertyName("lastapp")]
    public string? LastApplication { get; set; }

    /// <summary>Last application data.</summary>
    [JsonPropertyName("lastdata")]
    public string? LastData { get; set; }

    /// <summary>Total call duration in seconds.</summary>
    [JsonPropertyName("duration")]
    public int? Duration { get; set; }

    /// <summary>Billable seconds.</summary>
    [JsonPropertyName("billsec")]
    public int? BillableSeconds { get; set; }

    /// <summary>Disposition, for example <c>ANSWERED</c> or <c>NO ANSWER</c>.</summary>
    [JsonPropertyName("disposition")]
    public string? Disposition { get; set; }

    /// <summary>AMA flags.</summary>
    [JsonPropertyName("amaflags")]
    public string? AmaFlags { get; set; }

    /// <summary>Account code.</summary>
    [JsonPropertyName("accountcode")]
    public string? AccountCode { get; set; }

    /// <summary>User field.</summary>
    [JsonPropertyName("userfield")]
    public string? UserField { get; set; }

    /// <summary>DID.</summary>
    [JsonPropertyName("did")]
    public string? Did { get; set; }

    /// <summary>Recording file.</summary>
    [JsonPropertyName("recordingfile")]
    public string? RecordingFile { get; set; }

    /// <summary>Caller number.</summary>
    [JsonPropertyName("cnum")]
    public string? CallerNumber { get; set; }

    /// <summary>Outbound caller number.</summary>
    [JsonPropertyName("outbound_cnum")]
    public string? OutboundCallerNumber { get; set; }

    /// <summary>Outbound caller name.</summary>
    [JsonPropertyName("outbound_cnam")]
    public string? OutboundCallerName { get; set; }

    /// <summary>Destination caller name.</summary>
    [JsonPropertyName("dst_cnam")]
    public string? DestinationCallerName { get; set; }

    /// <summary>Linked id.</summary>
    [JsonPropertyName("linkedid")]
    public string? LinkedId { get; set; }

    /// <summary>Peer account.</summary>
    [JsonPropertyName("peeraccount")]
    public string? PeerAccount { get; set; }

    /// <summary>Sequence.</summary>
    [JsonPropertyName("sequence")]
    public string? Sequence { get; set; }
}

/// <summary>
/// Sort order for CDR queries.
/// </summary>
public enum CdrOrderBy
{
    /// <summary>Order by call date.</summary>
    Date,

    /// <summary>Order by duration.</summary>
    Duration
}

/// <summary>
/// Represents a FreePBX queue. Queues are not exposed through the GraphQL API,
/// so this model is populated from the REST API.
/// </summary>
public class QueueDto
{
    /// <summary>Queue number.</summary>
    [JsonPropertyName("extension")]
    public string Extension { get; set; } = string.Empty;

    /// <summary>Queue name.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>Static members, populated when fetching a single queue or its members.</summary>
    [JsonPropertyName("member")]
    public List<string>? Members { get; set; }

    /// <summary>Dynamic members, populated when fetching a single queue or its members.</summary>
    [JsonPropertyName("dynmembers")]
    public List<string>? DynamicMembers { get; set; }

    /// <summary>Any additional queue settings returned by the REST API.</summary>
    [JsonExtensionData]
    public Dictionary<string, System.Text.Json.JsonElement>? AdditionalSettings { get; set; }
}

/// <summary>
/// Static and dynamic members of a queue.
/// </summary>
public class QueueMembers
{
    /// <summary>Static members (queue configuration).</summary>
    [JsonPropertyName("member")]
    public List<string> Static { get; set; } = [];

    /// <summary>Dynamic members (added at runtime via Asterisk).</summary>
    [JsonPropertyName("dynmembers")]
    public List<string> Dynamic { get; set; } = [];

    /// <summary>Total number of members.</summary>
    [JsonIgnore]
    public int Count => Static.Count + Dynamic.Count;

    /// <summary>Creates an empty member set.</summary>
    public static QueueMembers Empty() => new();
}
