using System.Text.Json;

namespace Sengsara.Freepbx.Abstractions.Interfaces;

/// <summary>
/// Interface for executing GraphQL queries against FreePBX
/// </summary>
public interface IGraphQLExecutor
{
    /// <summary>
    /// Executes a GraphQL query and returns the deserialized result
    /// </summary>
    /// <typeparam name="TResponse">Expected response type</typeparam>
    /// <param name="query">GraphQL query string</param>
    /// <param name="variables">Query variables (optional)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deserialized response</returns>
    Task<TResponse> ExecuteQueryAsync<TResponse>(
        string query,
        Dictionary<string, object?>? variables = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a GraphQL mutation and returns the deserialized result
    /// </summary>
    /// <typeparam name="TResponse">Expected response type</typeparam>
    /// <param name="mutation">GraphQL mutation string</param>
    /// <param name="variables">Mutation variables (optional)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deserialized response</returns>
    Task<TResponse> ExecuteMutationAsync<TResponse>(
        string mutation,
        Dictionary<string, object?>? variables = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a raw GraphQL request and returns the raw JSON response
    /// </summary>
    /// <param name="query">GraphQL query or mutation</param>
    /// <param name="variables">Request variables (optional)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Raw JSON response</returns>
    Task<JsonDocument> ExecuteRawAsync(
        string query,
        Dictionary<string, object?>? variables = null,
        CancellationToken cancellationToken = default);
}