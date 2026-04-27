using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sengsara.Freepbx.Abstractions.Interfaces;
using Sengsara.Freepbx.Abstractions.Interfaces.Services;
using Sengsara.Freepbx.GraphQL;
using Sengsara.Freepbx.Services;

namespace Sengsara.Freepbx.Client;

/// <summary>
/// Main client for interacting with FreePBX API
/// </summary>
public class FreepbxClient : IFreepbxClient, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly GraphQLExecutor _graphQLExecutor;
    private readonly Services.ExtensionService _extensionService;
    private readonly Services.QueueService _queueService;
    private bool _disposed;

    /// <inheritdoc />
    public Abstractions.Interfaces.FreepbxClientOptions Options { get; }

    /// <inheritdoc />
    public IGraphQLExecutor GraphQL => _graphQLExecutor;

    /// <summary>
    /// Creates a new FreePBX client with the specified options
    /// </summary>
    /// <param name="options">Client configuration options</param>
    /// <param name="logger">Optional logger</param>
    public FreepbxClient(Client.FreepbxClientOptions options, ILogger<FreepbxClient>? logger = null)
    {
        Options = options ?? throw new ArgumentNullException(nameof(options));

        _httpClient = CreateHttpClient(options);
        _graphQLExecutor = new GraphQLExecutor(_httpClient, options, null);

        _extensionService = new Services.ExtensionService(this, null);
        _queueService = new Services.QueueService(this, null);
    }

    /// <summary>
    /// Gets the extension service
    /// </summary>
    public Abstractions.Interfaces.Services.IExtensionService Extensions => _extensionService;

    /// <summary>
    /// Gets the queue service
    /// </summary>
    public Abstractions.Interfaces.Services.IQueueService Queues => _queueService;

    private static HttpClient CreateHttpClient(Abstractions.Interfaces.FreepbxClientOptions options)
    {
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(options.Endpoint),
            Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds)
        };

        // Add authentication headers
        if (!string.IsNullOrWhiteSpace(options.ApiKey))
        {
            httpClient.DefaultRequestHeaders.Add("X-API-Key", options.ApiKey);
        }

        // Add basic auth if credentials provided
        if (!string.IsNullOrWhiteSpace(options.Username) && !string.IsNullOrWhiteSpace(options.Password))
        {
            var credentials = Convert.ToBase64String(
                System.Text.Encoding.ASCII.GetBytes($"{options.Username}:{options.Password}"));
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
        }

        return httpClient;
    }

    /// <inheritdoc />
    public async Task<bool> TestConnectionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Execute a simple introspection query to test the connection
            var query = @"query { __schema { queryType { name } } }";
            await _graphQLExecutor.ExecuteRawAsync(query, cancellationToken: cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <inheritdoc />
    public async Task DisposeAsync()
    {
        if (!_disposed)
        {
            _httpClient.Dispose();
            _disposed = true;
        }

        await Task.CompletedTask;
    }

    /// <summary>
    /// Disposes the client
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            _httpClient.Dispose();
            _disposed = true;
        }

        GC.SuppressFinalize(this);
    }
}