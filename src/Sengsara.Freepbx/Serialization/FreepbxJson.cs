using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sengsara.Freepbx.Serialization;

/// <summary>
/// Shared JSON settings used across the library.
/// </summary>
internal static class FreepbxJson
{
    /// <summary>Options for deserializing FreePBX responses.</summary>
    public static readonly JsonSerializerOptions Response = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        NumberHandling = JsonNumberHandling.AllowReadingFromString
    };

    /// <summary>Options for serializing request bodies and GraphQL variables.</summary>
    public static readonly JsonSerializerOptions Request = new(JsonSerializerDefaults.Web)
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        DictionaryKeyPolicy = null
    };
}
