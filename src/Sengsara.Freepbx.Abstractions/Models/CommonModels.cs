using Sengsara.Freepbx.Abstractions.Exceptions;

namespace Sengsara.Freepbx.Abstractions.Models;

/// <summary>
/// Represents a GraphQL response that may contain both data and errors, for
/// example when FreePBX resolves a non-existent object and reports errors for
/// the nested fields while still returning a partial payload.
/// </summary>
/// <typeparam name="T">Data type.</typeparam>
public sealed class GraphQLResult<T>
{
    /// <summary>Deserialized data, when present.</summary>
    public T? Data { get; init; }

    /// <summary>Errors returned by the server.</summary>
    public IReadOnlyList<GraphQLError> Errors { get; init; } = [];

    /// <summary>True when the server reported at least one error.</summary>
    public bool HasErrors => Errors.Count > 0;

    /// <summary>Raw response body.</summary>
    public string? RawBody { get; init; }
}

/// <summary>
/// Result of a FreePBX mutation. FreePBX mutations generally return a
/// <c>status</c> flag and an optional <c>message</c>.
/// </summary>
public class MutationResult
{
    /// <summary>
    /// True when the mutation succeeded.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Optional human readable message returned by FreePBX.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    public static MutationResult Ok(string? message = null) => new() { Success = true, Message = message };

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    public static MutationResult Fail(string? message = null) => new() { Success = false, Message = message };

    /// <inheritdoc />
    public override string ToString() => Success ? $"Success: {Message}" : $"Failure: {Message}";
}

/// <summary>
/// Relay style pagination information.
/// </summary>
public class PageInfo
{
    /// <summary>Whether more items exist after the current page.</summary>
    public bool HasNextPage { get; set; }

    /// <summary>Whether more items exist before the current page.</summary>
    public bool HasPreviousPage { get; set; }

    /// <summary>Cursor of the first item on the page.</summary>
    public string? StartCursor { get; set; }

    /// <summary>Cursor of the last item on the page.</summary>
    public string? EndCursor { get; set; }
}

/// <summary>
/// A page of results returned from a Relay style connection.
/// </summary>
/// <typeparam name="T">Item type.</typeparam>
public class ConnectionResult<T>
{
    /// <summary>Items in the current page.</summary>
    public IReadOnlyList<T> Items { get; init; } = [];

    /// <summary>Total number of items available, when reported by the server.</summary>
    public int? TotalCount { get; init; }

    /// <summary>Number of items returned in this response, when reported.</summary>
    public int? Count { get; init; }

    /// <summary>Pagination information, when reported by the server.</summary>
    public PageInfo? PageInfo { get; init; }

    /// <summary>Optional status message returned by FreePBX.</summary>
    public string? Message { get; init; }
}
