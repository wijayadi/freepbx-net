using System.Text.Json.Serialization;

namespace Sengsara.Freepbx.Abstractions.Models;

/// <summary>
/// Represents a FreePBX inbound route (DID).
/// </summary>
public class InboundRouteDto
{
    /// <summary>Relay global identifier.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>DID / extension number.</summary>
    [JsonPropertyName("extension")]
    public string Extension { get; set; } = string.Empty;

    /// <summary>Caller ID number filter.</summary>
    [JsonPropertyName("cidnum")]
    public string? CidNum { get; set; }

    /// <summary>Description.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>Require caller privacy.</summary>
    [JsonPropertyName("privacyman")]
    public bool? PrivacyManager { get; set; }

    /// <summary>Alert info.</summary>
    [JsonPropertyName("alertinfo")]
    public string? AlertInfo { get; set; }

    /// <summary>Enable distinctive ringing.</summary>
    [JsonPropertyName("ringing")]
    public bool? Ringing { get; set; }

    /// <summary>Music on hold class.</summary>
    [JsonPropertyName("mohclass")]
    public string? MohClass { get; set; }

    /// <summary>Caller ID prefix.</summary>
    [JsonPropertyName("grppre")]
    public string? GroupPrefix { get; set; }

    /// <summary>Delay before answering, in seconds.</summary>
    [JsonPropertyName("delay_answer")]
    public int? DelayAnswer { get; set; }

    /// <summary>CID priority route.</summary>
    [JsonPropertyName("pricid")]
    public bool? PriorityCid { get; set; }

    /// <summary>Privacy manager max retries.</summary>
    [JsonPropertyName("pmmaxretries")]
    public string? PmMaxRetries { get; set; }

    /// <summary>Privacy manager minimum length.</summary>
    [JsonPropertyName("pmminlength")]
    public string? PmMinLength { get; set; }

    /// <summary>Reversal.</summary>
    [JsonPropertyName("reversal")]
    public bool? Reversal { get; set; }

    /// <summary>Ringer volume.</summary>
    [JsonPropertyName("rvolume")]
    public string? RVolume { get; set; }

    /// <summary>Force answer.</summary>
    [JsonPropertyName("fanswer")]
    public bool? ForceAnswer { get; set; }

    /// <summary>Destination description, for example <c>Extensions:101</c>.</summary>
    [JsonPropertyName("destinationConnection")]
    public string? DestinationConnection { get; set; }
}

/// <summary>
/// Request for <c>addInboundRoute</c>.
/// </summary>
public class AddInboundRouteRequest
{
    /// <summary>DID / extension number.</summary>
    [JsonPropertyName("extension")]
    public string? Extension { get; set; }

    /// <summary>Caller ID number filter.</summary>
    [JsonPropertyName("cidnum")]
    public string? CidNum { get; set; }

    /// <summary>Description.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>Require caller privacy.</summary>
    [JsonPropertyName("privacyman")]
    public bool? PrivacyManager { get; set; }

    /// <summary>Alert info.</summary>
    [JsonPropertyName("alertinfo")]
    public string? AlertInfo { get; set; }

    /// <summary>Distinctive ringing.</summary>
    [JsonPropertyName("ringing")]
    public bool? Ringing { get; set; }

    /// <summary>Music on hold class.</summary>
    [JsonPropertyName("mohclass")]
    public string? MohClass { get; set; }

    /// <summary>Caller ID prefix.</summary>
    [JsonPropertyName("grppre")]
    public string? GroupPrefix { get; set; }

    /// <summary>Delay before answering, in seconds.</summary>
    [JsonPropertyName("delay_answer")]
    public int? DelayAnswer { get; set; }

    /// <summary>CID priority route.</summary>
    [JsonPropertyName("pricid")]
    public bool? PriorityCid { get; set; }

    /// <summary>Privacy manager max retries.</summary>
    [JsonPropertyName("pmmaxretries")]
    public string? PmMaxRetries { get; set; }

    /// <summary>Privacy manager minimum length.</summary>
    [JsonPropertyName("pmminlength")]
    public string? PmMinLength { get; set; }

    /// <summary>Reversal.</summary>
    [JsonPropertyName("reversal")]
    public bool? Reversal { get; set; }

    /// <summary>Ringer volume.</summary>
    [JsonPropertyName("rvolume")]
    public string? RVolume { get; set; }

    /// <summary>Force answer.</summary>
    [JsonPropertyName("fanswer")]
    public bool? ForceAnswer { get; set; }

    /// <summary>Destination in FreePBX format, for example <c>ext-local,101,1</c> (required).</summary>
    [JsonPropertyName("destination")]
    public required string Destination { get; set; }
}

/// <summary>
/// Request for <c>updateInboundRoute</c>.
/// </summary>
public class UpdateInboundRouteRequest
{
    /// <summary>DID / extension number (required).</summary>
    [JsonPropertyName("extension")]
    public required string Extension { get; set; }

    /// <summary>Caller ID number filter.</summary>
    [JsonPropertyName("cidnum")]
    public string? CidNum { get; set; }

    /// <summary>Previous extension (when renaming).</summary>
    [JsonPropertyName("oldExtension")]
    public string? OldExtension { get; set; }

    /// <summary>Previous caller ID filter (when renaming).</summary>
    [JsonPropertyName("oldCidnum")]
    public string? OldCidNum { get; set; }

    /// <summary>Description.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>Require caller privacy.</summary>
    [JsonPropertyName("privacyman")]
    public bool? PrivacyManager { get; set; }

    /// <summary>Alert info.</summary>
    [JsonPropertyName("alertinfo")]
    public string? AlertInfo { get; set; }

    /// <summary>Distinctive ringing.</summary>
    [JsonPropertyName("ringing")]
    public bool? Ringing { get; set; }

    /// <summary>Music on hold class.</summary>
    [JsonPropertyName("mohclass")]
    public string? MohClass { get; set; }

    /// <summary>Caller ID prefix.</summary>
    [JsonPropertyName("grppre")]
    public string? GroupPrefix { get; set; }

    /// <summary>Delay before answering, in seconds.</summary>
    [JsonPropertyName("delay_answer")]
    public int? DelayAnswer { get; set; }

    /// <summary>CID priority route.</summary>
    [JsonPropertyName("pricid")]
    public bool? PriorityCid { get; set; }

    /// <summary>Privacy manager max retries.</summary>
    [JsonPropertyName("pmmaxretries")]
    public string? PmMaxRetries { get; set; }

    /// <summary>Privacy manager minimum length.</summary>
    [JsonPropertyName("pmminlength")]
    public string? PmMinLength { get; set; }

    /// <summary>Reversal.</summary>
    [JsonPropertyName("reversal")]
    public bool? Reversal { get; set; }

    /// <summary>Ringer volume.</summary>
    [JsonPropertyName("rvolume")]
    public string? RVolume { get; set; }

    /// <summary>Force answer.</summary>
    [JsonPropertyName("fanswer")]
    public bool? ForceAnswer { get; set; }

    /// <summary>Destination in FreePBX format (required).</summary>
    [JsonPropertyName("destination")]
    public required string Destination { get; set; }
}
