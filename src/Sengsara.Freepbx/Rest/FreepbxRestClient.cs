using System.Net;
using System.Text;
using System.Text.Json;
using Sengsara.Freepbx.Abstractions.Exceptions;
using Sengsara.Freepbx.Abstractions.Interfaces;
using Sengsara.Freepbx.Serialization;

namespace Sengsara.Freepbx.Rest;

/// <summary>
/// Client for the FreePBX REST API.
/// </summary>
public sealed class FreepbxRestClient : IFreepbxRestClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    /// <summary>
    /// Creates a new REST client.
    /// </summary>
    /// <param name="httpClient">Authenticated HTTP client.</param>
    /// <param name="options">Client options.</param>
    public FreepbxRestClient(HttpClient httpClient, FreepbxClientOptions options)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _baseUrl = options.RestBaseUri.ToString().TrimEnd('/');
    }

    /// <inheritdoc />
    public Task<TResponse> GetAsync<TResponse>(string path, CancellationToken cancellationToken = default)
        => SendAsync<TResponse>(HttpMethod.Get, path, null, cancellationToken);

    /// <inheritdoc />
    public Task<TResponse> PutAsync<TResponse>(string path, object? body, CancellationToken cancellationToken = default)
        => SendAsync<TResponse>(HttpMethod.Put, path, body, cancellationToken);

    /// <inheritdoc />
    public Task<TResponse> PostAsync<TResponse>(string path, object? body, CancellationToken cancellationToken = default)
        => SendAsync<TResponse>(HttpMethod.Post, path, body, cancellationToken);

    /// <inheritdoc />
    public async Task<JsonDocument> SendRawAsync(HttpMethod method, string path, object? body = null, CancellationToken cancellationToken = default)
    {
        using var response = await SendRequestAsync(method, path, body, cancellationToken).ConfigureAwait(false);
        var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        return JsonDocument.Parse(string.IsNullOrWhiteSpace(content) ? "{}" : content);
    }

    private async Task<TResponse> SendAsync<TResponse>(HttpMethod method, string path, object? body, CancellationToken cancellationToken)
    {
        using var response = await SendRequestAsync(method, path, body, cancellationToken).ConfigureAwait(false);
        var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        if (string.IsNullOrWhiteSpace(content) || content == "false" || content == "null")
        {
            return default!;
        }

        try
        {
            return JsonSerializer.Deserialize<TResponse>(content, FreepbxJson.Response)!;
        }
        catch (JsonException ex)
        {
            throw new FreepbxRestException(
                $"Unable to deserialize the FreePBX REST response into {typeof(TResponse).Name}: {ex.Message}",
                (int)response.StatusCode,
                content);
        }
    }

    private async Task<HttpResponseMessage> SendRequestAsync(HttpMethod method, string path, object? body, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(method, BuildUri(path));

        if (body is not null)
        {
            var json = JsonSerializer.Serialize(body, FreepbxJson.Request);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

        if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
        {
            var forbiddenBody = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            response.Dispose();
            throw new FreepbxRestException(
                $"FreePBX REST request to '{path}' was not authorized. Ensure the OAuth client has the required 'rest' scopes.",
                (int)HttpStatusCode.Forbidden,
                forbiddenBody);
        }

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            response.Dispose();
            throw new FreepbxRestException(
                $"FreePBX REST request to '{path}' failed with status {(int)response.StatusCode}.",
                (int)response.StatusCode,
                errorBody);
        }

        return response;
    }

    private string BuildUri(string path) => _baseUrl + "/" + path.TrimStart('/');
}
