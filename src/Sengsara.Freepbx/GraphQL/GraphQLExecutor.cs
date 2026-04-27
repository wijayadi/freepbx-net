using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sengsara.Freepbx.Abstractions.Interfaces;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using GraphQLRequest = GraphQL.GraphQLRequest;
using GraphQLError = GraphQL.GraphQLError;

namespace Sengsara.Freepbx.GraphQL;

/// <summary>
/// Executes GraphQL queries against the FreePBX API
/// </summary>
public class GraphQLExecutor : IGraphQLExecutor
{
    private readonly GraphQLHttpClient _client;
    private readonly ILogger<GraphQLExecutor>? _logger;

    public GraphQLExecutor(HttpClient httpClient, Abstractions.Interfaces.FreepbxClientOptions options, ILogger<GraphQLExecutor>? logger = null)
    {
        _logger = logger;

        var serializer = new SystemTextJsonSerializer(new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        var clientOptions = new GraphQLHttpClientOptions
        {
            EndPoint = new Uri(options.Endpoint)
        };
        
        _client = new GraphQLHttpClient(clientOptions, serializer, httpClient);

        // Add authentication
        if (!string.IsNullOrWhiteSpace(options.ApiKey))
        {
            _client.HttpClient.DefaultRequestHeaders.Add("X-API-Key", options.ApiKey);
        }
    }

    /// <inheritdoc />
    public async Task<TResponse> ExecuteQueryAsync<TResponse>(
        string query,
        Dictionary<string, object?>? variables = null,
        CancellationToken cancellationToken = default)
    {
        _logger?.LogDebug("Executing GraphQL query: {Query}", query);

        var request = new GraphQLRequest
        {
            Query = query,
            Variables = variables
        };

        var response = await _client.SendQueryAsync<TResponse>(request, cancellationToken);

        if (response.Errors?.Any() == true)
        {
            var errorMessage = string.Join(", ", response.Errors.Select(e => e.Message));
            _logger?.LogError("GraphQL query errors: {Errors}", errorMessage);
            throw new GraphQLException(response.Errors.Select(e => new GraphQLError { Message = e.Message }).ToList());
        }

        return response.Data;
    }

    /// <inheritdoc />
    public async Task<TResponse> ExecuteMutationAsync<TResponse>(
        string mutation,
        Dictionary<string, object?>? variables = null,
        CancellationToken cancellationToken = default)
    {
        _logger?.LogDebug("Executing GraphQL mutation: {Mutation}", mutation);

        var request = new GraphQLRequest
        {
            Query = mutation,
            Variables = variables
        };

        var response = await _client.SendMutationAsync<TResponse>(request, cancellationToken);

        if (response.Errors?.Any() == true)
        {
            var errorMessage = string.Join(", ", response.Errors.Select(e => e.Message));
            _logger?.LogError("GraphQL mutation errors: {Errors}", errorMessage);
            throw new GraphQLException(response.Errors.Select(e => new GraphQLError { Message = e.Message }).ToList());
        }

        return response.Data;
    }

    /// <inheritdoc />
    public async Task<JsonDocument> ExecuteRawAsync(
        string query,
        Dictionary<string, object?>? variables = null,
        CancellationToken cancellationToken = default)
    {
        _logger?.LogDebug("Executing raw GraphQL request: {Query}", query);

        var request = new GraphQLRequest
        {
            Query = query,
            Variables = variables
        };

        var response = await _client.SendQueryAsync<JsonDocument>(request, cancellationToken);

        if (response.Errors?.Any() == true)
        {
            var errorMessage = string.Join(", ", response.Errors.Select(e => e.Message));
            _logger?.LogError("GraphQL request errors: {Errors}", errorMessage);
            throw new GraphQLException(response.Errors.Select(e => new GraphQLError { Message = e.Message }).ToList());
        }

        return response.Data;
    }
}

/// <summary>
/// Exception thrown when GraphQL errors occur
/// </summary>
public class GraphQLException : Exception
{
    public IReadOnlyList<GraphQLError> Errors { get; }

    public GraphQLException(IReadOnlyList<GraphQLError> errors)
        : base(string.Join(", ", errors.Select(e => e.Message)))
    {
        Errors = errors;
    }
}

/// <summary>
/// Represents a GraphQL error
/// </summary>
public class GraphQLError
{
    public string Message { get; set; } = string.Empty;
    public string? Path { get; set; }
    public List<object>? Locations { get; set; }
}