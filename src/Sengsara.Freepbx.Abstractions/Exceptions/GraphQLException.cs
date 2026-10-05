using System.Text.Json.Serialization;

namespace Sengsara.Freepbx.Abstractions.Exceptions;

/// <summary>
/// Represents a single error returned inside a GraphQL response.
/// </summary>
public class GraphQLError
{
    /// <summary>Error message.</summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>Path to the field that produced the error, if provided.</summary>
    [JsonPropertyName("path")]
    public List<object>? Path { get; set; }

    /// <summary>Source locations of the error, if provided.</summary>
    [JsonPropertyName("locations")]
    public List<GraphQLErrorLocation>? Locations { get; set; }

    /// <summary>Additional error metadata, if provided.</summary>
    [JsonPropertyName("extensions")]
    public Dictionary<string, object>? Extensions { get; set; }
}

/// <summary>
/// Source location of a GraphQL error.
/// </summary>
public class GraphQLErrorLocation
{
    /// <summary>Line number.</summary>
    [JsonPropertyName("line")]
    public int Line { get; set; }

    /// <summary>Column number.</summary>
    [JsonPropertyName("column")]
    public int Column { get; set; }
}

/// <summary>
/// Thrown when the FreePBX GraphQL API returns one or more errors.
/// </summary>
public class GraphQLException : Exception
{
    /// <summary>Errors returned by the server.</summary>
    public IReadOnlyList<GraphQLError> Errors { get; }

    /// <summary>Raw response body, when available.</summary>
    public string? ResponseBody { get; }

    /// <summary>HTTP status code associated with the response, when available.</summary>
    public int? StatusCode { get; }

    /// <summary>
    /// Creates a new GraphQL exception.
    /// </summary>
    public GraphQLException(IReadOnlyList<GraphQLError> errors, string? responseBody = null, int? statusCode = null)
        : base(BuildMessage(errors))
    {
        Errors = errors;
        ResponseBody = responseBody;
        StatusCode = statusCode;
    }

    private static string BuildMessage(IReadOnlyList<GraphQLError> errors)
        => errors.Count == 0 ? "GraphQL request failed." : string.Join("; ", errors.Select(e => e.Message));
}

/// <summary>
/// Thrown when a FreePBX REST request fails.
/// </summary>
public class FreepbxRestException : Exception
{
    /// <summary>HTTP status code.</summary>
    public int StatusCode { get; }

    /// <summary>Raw response body.</summary>
    public string? ResponseBody { get; }

    /// <summary>
    /// Creates a new REST exception.
    /// </summary>
    public FreepbxRestException(string message, int statusCode, string? responseBody = null)
        : base(message)
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
    }
}
