using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Sengsara.Freepbx.Abstractions.Exceptions;
using Sengsara.Freepbx.Abstractions.Interfaces;
using Sengsara.Freepbx.Abstractions.Models;
using Sengsara.Freepbx.Serialization;

namespace Sengsara.Freepbx.GraphQL;

/// <summary>
/// Executes GraphQL operations against the FreePBX GraphQL endpoint.
/// </summary>
public sealed class GraphQLExecutor : IGraphQLExecutor
{
    private readonly HttpClient _httpClient;
    private readonly Uri _endpoint;
    private readonly ILogger<GraphQLExecutor>? _logger;
    private readonly bool _retry;
    private readonly int _maxRetryAttempts;

    /// <summary>
    /// Creates a new GraphQL executor.
    /// </summary>
    /// <param name="httpClient">Authenticated HTTP client.</param>
    /// <param name="options">Client options.</param>
    /// <param name="logger">Optional logger.</param>
    public GraphQLExecutor(HttpClient httpClient, FreepbxClientOptions options, ILogger<GraphQLExecutor>? logger = null)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _endpoint = options.GraphQLUri;
        _logger = logger;
        _retry = options.EnableRetry;
        _maxRetryAttempts = options.MaxRetryAttempts;
    }

    /// <inheritdoc />
    public Task<TResponse> ExecuteQueryAsync<TResponse>(string query, object? variables = null, CancellationToken cancellationToken = default)
        => ExecuteAsync<TResponse>(query, variables, cancellationToken);

    /// <inheritdoc />
    public Task<TResponse> ExecuteMutationAsync<TResponse>(string mutation, object? variables = null, CancellationToken cancellationToken = default)
        => ExecuteAsync<TResponse>(mutation, variables, cancellationToken);

    /// <inheritdoc />
    public async Task<GraphQLResult<TResponse>> TryExecuteQueryAsync<TResponse>(string query, object? variables = null, CancellationToken cancellationToken = default)
    {
        var response = await SendAsync(query, variables, cancellationToken).ConfigureAwait(false);
        var errors = ReadErrors(response.Root);

        TResponse? data = default;
        if (response.Root.TryGetProperty("data", out var dataElement) &&
            dataElement.ValueKind is not JsonValueKind.Null and not JsonValueKind.Undefined)
        {
            data = dataElement.Deserialize<TResponse>(FreepbxJson.Response);
        }

        return new GraphQLResult<TResponse>
        {
            Data = data,
            Errors = errors,
            RawBody = response.Body
        };
    }

    /// <inheritdoc />
    public async Task<JsonDocument> ExecuteRawAsync(string query, object? variables = null, CancellationToken cancellationToken = default)
    {
        var response = await SendAsync(query, variables, cancellationToken).ConfigureAwait(false);
        ThrowIfErrors(response);

        if (response.Root.TryGetProperty("data", out var data) &&
            data.ValueKind is not JsonValueKind.Null and not JsonValueKind.Undefined)
        {
            return JsonDocument.Parse(data.GetRawText());
        }

        return JsonDocument.Parse("{}");
    }

    private async Task<TResponse> ExecuteAsync<TResponse>(string query, object? variables, CancellationToken cancellationToken)
    {
        var response = await SendAsync(query, variables, cancellationToken).ConfigureAwait(false);
        ThrowIfErrors(response);

        if (!response.Root.TryGetProperty("data", out var data) ||
            data.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
        {
            throw new GraphQLException(
                [new GraphQLError { Message = "The FreePBX GraphQL response did not contain any data." }],
                response.Body,
                response.StatusCode);
        }

        var result = data.Deserialize<TResponse>(FreepbxJson.Response);
        if (result is null)
        {
            throw new GraphQLException(
                [new GraphQLError { Message = $"Unable to deserialize the GraphQL response into {typeof(TResponse).Name}." }],
                response.Body,
                response.StatusCode);
        }

        return result;
    }

    private async Task<GraphQLResponseEnvelope> SendAsync(string query, object? variables, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            throw new ArgumentException("GraphQL query cannot be empty.", nameof(query));
        }

        var payload = new GraphQLRequestPayload
        {
            Query = query,
            Variables = variables
        };

        var json = JsonSerializer.Serialize(payload, FreepbxJson.Request);
        _logger?.LogDebug("Executing GraphQL request: {Query}", query);

        var attempts = _retry ? Math.Max(1, _maxRetryAttempts + 1) : 1;
        Exception? lastException = null;

        for (var attempt = 1; attempt <= attempts; attempt++)
        {
            try
            {
                using var content = new StringContent(json, Encoding.UTF8, "application/json");
                using var response = await _httpClient.PostAsync(_endpoint, content, cancellationToken).ConfigureAwait(false);
                var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

                if (IsTransient(response.StatusCode) && attempt < attempts)
                {
                    _logger?.LogWarning("GraphQL request returned {StatusCode}; retrying ({Attempt}/{Attempts}).",
                        (int)response.StatusCode, attempt, attempts);
                    await DelayAsync(attempt, cancellationToken).ConfigureAwait(false);
                    continue;
                }

                return ParseJson(body, (int)response.StatusCode);
            }
            catch (HttpRequestException ex) when (attempt < attempts)
            {
                lastException = ex;
                _logger?.LogWarning(ex, "GraphQL request failed; retrying ({Attempt}/{Attempts}).", attempt, attempts);
                await DelayAsync(attempt, cancellationToken).ConfigureAwait(false);
            }
            catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested && attempt < attempts)
            {
                lastException = ex;
                _logger?.LogWarning(ex, "GraphQL request timed out; retrying ({Attempt}/{Attempts}).", attempt, attempts);
                await DelayAsync(attempt, cancellationToken).ConfigureAwait(false);
            }
        }

        throw new GraphQLException(
            [new GraphQLError { Message = lastException?.Message ?? "GraphQL request failed after retries." }]);
    }

    private static GraphQLResponseEnvelope ParseJson(string body, int statusCode)
    {
        try
        {
            using var document = JsonDocument.Parse(body);
            return new GraphQLResponseEnvelope(document.RootElement.Clone(), body, statusCode);
        }
        catch (JsonException ex)
        {
            throw new GraphQLException(
                [new GraphQLError { Message = $"FreePBX returned an invalid JSON response: {ex.Message}" }],
                body,
                statusCode);
        }
    }

    private static List<GraphQLError> ReadErrors(JsonElement root)
    {
        if (root.TryGetProperty("errors", out var errors) && errors.ValueKind == JsonValueKind.Array && errors.GetArrayLength() > 0)
        {
            return errors.Deserialize<List<GraphQLError>>(FreepbxJson.Response) ?? [];
        }

        return [];
    }

    private static void ThrowIfErrors(GraphQLResponseEnvelope response)
    {
        var errors = ReadErrors(response.Root);
        if (errors.Count > 0)
        {
            throw new GraphQLException(errors, response.Body, response.StatusCode);
        }

        if (!response.Root.TryGetProperty("data", out _) && response.StatusCode >= 400)
        {
            throw new GraphQLException(
                [new GraphQLError { Message = $"FreePBX returned HTTP {response.StatusCode}." }],
                response.Body,
                response.StatusCode);
        }
    }

    private static bool IsTransient(HttpStatusCode statusCode)
        => (int)statusCode >= 500 || statusCode == HttpStatusCode.RequestTimeout;

    private static Task DelayAsync(int attempt, CancellationToken cancellationToken)
        => Task.Delay(TimeSpan.FromMilliseconds(200 * Math.Pow(2, attempt - 1)), cancellationToken);

    private sealed class GraphQLRequestPayload
    {
        public string Query { get; set; } = string.Empty;

        public object? Variables { get; set; }

        public string? OperationName { get; set; }
    }

    private sealed record GraphQLResponseEnvelope(JsonElement Root, string Body, int StatusCode);
}
