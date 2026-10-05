using System.Text.Json;
using Sengsara.Freepbx.Abstractions.Models;

namespace Sengsara.Freepbx.Abstractions.Interfaces;

/// <summary>
/// Executes GraphQL operations against the FreePBX GraphQL endpoint.
/// </summary>
public interface IGraphQLExecutor
{
    /// <summary>
    /// Executes a GraphQL query and returns the deserialized <c>data</c> payload.
    /// </summary>
    /// <typeparam name="TResponse">Expected data type.</typeparam>
    /// <param name="query">GraphQL query document.</param>
    /// <param name="variables">Optional variables object (anonymous type or dictionary).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<TResponse> ExecuteQueryAsync<TResponse>(
        string query,
        object? variables = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a GraphQL mutation and returns the deserialized <c>data</c> payload.
    /// </summary>
    /// <typeparam name="TResponse">Expected data type.</typeparam>
    /// <param name="mutation">GraphQL mutation document.</param>
    /// <param name="variables">Optional variables object (anonymous type or dictionary).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<TResponse> ExecuteMutationAsync<TResponse>(
        string mutation,
        object? variables = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a GraphQL query and returns both the data and any errors without
    /// throwing. Useful for lookups where FreePBX reports per-field errors for a
    /// missing object while still returning a partial payload.
    /// </summary>
    /// <typeparam name="TResponse">Expected data type.</typeparam>
    /// <param name="query">GraphQL query document.</param>
    /// <param name="variables">Optional variables object.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<GraphQLResult<TResponse>> TryExecuteQueryAsync<TResponse>(
        string query,
        object? variables = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a GraphQL operation and returns the raw JSON <c>data</c> payload.
    /// </summary>
    /// <param name="query">GraphQL query or mutation document.</param>
    /// <param name="variables">Optional variables object.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<JsonDocument> ExecuteRawAsync(
        string query,
        object? variables = null,
        CancellationToken cancellationToken = default);
}
