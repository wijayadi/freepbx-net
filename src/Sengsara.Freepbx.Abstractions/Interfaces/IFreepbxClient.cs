namespace Sengsara.Freepbx.Abstractions.Interfaces;

/// <summary>
/// Main client interface for interacting with FreePBX API
/// </summary>
public interface IFreepbxClient
{
    /// <summary>
    /// Gets the GraphQL executor for direct queries
    /// </summary>
    IGraphQLExecutor GraphQL { get; }

    /// <summary>
    /// Gets the configured options
    /// </summary>
    FreepbxClientOptions Options { get; }

    /// <summary>
    /// Tests the connection to FreePBX
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if connection is successful</returns>
    Task<bool> TestConnectionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Disposes the client and releases resources
    /// </summary>
    Task DisposeAsync();
}

/// <summary>
/// Configuration options for FreePBX client
/// </summary>
public class FreepbxClientOptions
{
    /// <summary>
    /// Base URL of the FreePBX GraphQL endpoint
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>
    /// API key for authentication
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// Username for authentication
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Password for authentication
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Request timeout in seconds
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// Enable request retry on transient failures
    /// </summary>
    public bool EnableRetry { get; set; } = true;

    /// <summary>
    /// Maximum number of retry attempts
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 3;
}