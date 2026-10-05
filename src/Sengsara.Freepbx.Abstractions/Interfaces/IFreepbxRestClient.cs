using System.Text.Json;

namespace Sengsara.Freepbx.Abstractions.Interfaces;

/// <summary>
/// Low level client for the FreePBX REST API (<c>/admin/api/api/rest</c>).
/// </summary>
public interface IFreepbxRestClient
{
    /// <summary>
    /// Sends a GET request to the REST API and deserializes the response.
    /// </summary>
    /// <typeparam name="TResponse">Expected response type.</typeparam>
    /// <param name="path">Path relative to the REST root, for example <c>queues</c>.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<TResponse> GetAsync<TResponse>(string path, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a PUT request with a JSON body to the REST API and deserializes the response.
    /// </summary>
    /// <typeparam name="TResponse">Expected response type.</typeparam>
    /// <param name="path">Path relative to the REST root.</param>
    /// <param name="body">Request body.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<TResponse> PutAsync<TResponse>(string path, object? body, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a POST request with a JSON body to the REST API and deserializes the response.
    /// </summary>
    /// <typeparam name="TResponse">Expected response type.</typeparam>
    /// <param name="path">Path relative to the REST root.</param>
    /// <param name="body">Request body.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<TResponse> PostAsync<TResponse>(string path, object? body, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a request and returns the raw JSON payload.
    /// </summary>
    /// <param name="method">HTTP method.</param>
    /// <param name="path">Path relative to the REST root.</param>
    /// <param name="body">Optional request body.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<JsonDocument> SendRawAsync(HttpMethod method, string path, object? body = null, CancellationToken cancellationToken = default);
}
