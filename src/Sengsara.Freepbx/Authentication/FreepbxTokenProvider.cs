using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Sengsara.Freepbx.Abstractions.Exceptions;
using Sengsara.Freepbx.Abstractions.Interfaces;
using Sengsara.Freepbx.Http;

namespace Sengsara.Freepbx.Authentication;

/// <summary>
/// Acquires and caches OAuth2 bearer tokens from a FreePBX instance.
/// Supports the <c>client_credentials</c> and <c>password</c> grants as well as
/// a caller supplied static token.
/// </summary>
public sealed class FreepbxTokenProvider : ITokenProvider, IDisposable
{
    private readonly FreepbxClientOptions _options;
    private readonly HttpClient _httpClient;
    private readonly bool _ownsHttpClient;
    private readonly ILogger<FreepbxTokenProvider>? _logger;
    private readonly SemaphoreSlim _gate = new(1, 1);

    private string? _accessToken;
    private DateTimeOffset _expiresAt = DateTimeOffset.MinValue;
    private bool _disposed;

    /// <summary>
    /// Creates a token provider.
    /// </summary>
    /// <param name="options">Client options.</param>
    /// <param name="httpClient">Optional HttpClient used for token requests.</param>
    /// <param name="logger">Optional logger.</param>
    public FreepbxTokenProvider(
        FreepbxClientOptions options,
        HttpClient? httpClient = null,
        ILogger<FreepbxTokenProvider>? logger = null)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _httpClient = httpClient ?? FreepbxHttpClientFactory.CreateClient(options);
        _ownsHttpClient = httpClient is null;
        _logger = logger;

        if (!string.IsNullOrWhiteSpace(options.AccessToken))
        {
            _accessToken = options.AccessToken;
            _expiresAt = options.AccessTokenExpiresAt ?? DateTimeOffset.MaxValue;
        }
    }

    /// <inheritdoc />
    public async Task<string> GetAccessTokenAsync(bool forceRefresh = false, CancellationToken cancellationToken = default)
    {
        if (!forceRefresh && HasValidToken())
        {
            return _accessToken!;
        }

        await _gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (!forceRefresh && HasValidToken())
            {
                return _accessToken!;
            }

            if (!_options.CanAcquireToken)
            {
                if (!string.IsNullOrWhiteSpace(_options.AccessToken))
                {
                    _accessToken = _options.AccessToken;
                    _expiresAt = _options.AccessTokenExpiresAt ?? DateTimeOffset.MaxValue;
                    return _accessToken;
                }

                throw new InvalidOperationException(
                    "No OAuth credentials are configured. Provide ClientId/ClientSecret, Username/Password, or a static AccessToken.");
            }

            await AcquireAsync(cancellationToken).ConfigureAwait(false);
            return _accessToken!;
        }
        finally
        {
            _gate.Release();
        }
    }

    /// <inheritdoc />
    public void Invalidate()
    {
        _accessToken = null;
        _expiresAt = DateTimeOffset.MinValue;
    }

    private bool HasValidToken()
        => !string.IsNullOrEmpty(_accessToken) &&
           _expiresAt != DateTimeOffset.MinValue &&
           DateTimeOffset.UtcNow < _expiresAt;

    private async Task AcquireAsync(CancellationToken cancellationToken)
    {
        var hasClient = !string.IsNullOrWhiteSpace(_options.ClientId);
        var hasUser = !string.IsNullOrWhiteSpace(_options.Username) && !string.IsNullOrWhiteSpace(_options.Password);

        var form = new Dictionary<string, string>
        {
            ["client_id"] = _options.ClientId ?? string.Empty,
            ["scope"] = _options.Scope
        };

        if (!string.IsNullOrWhiteSpace(_options.ClientSecret))
        {
            form["client_secret"] = _options.ClientSecret!;
        }

        if (hasUser && hasClient)
        {
            form["grant_type"] = "password";
            form["username"] = _options.Username!;
            form["password"] = _options.Password!;
        }
        else
        {
            form["grant_type"] = "client_credentials";
        }

        _logger?.LogDebug("Requesting FreePBX access token using grant type {GrantType}", form["grant_type"]);

        using var response = await _httpClient
            .PostAsync(_options.TokenUri, new FormUrlEncodedContent(form), cancellationToken)
            .ConfigureAwait(false);

        var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            _logger?.LogError("FreePBX token request failed with {StatusCode}: {Body}", (int)response.StatusCode, body);
            throw new FreepbxRestException(
                $"FreePBX token request failed with status {(int)response.StatusCode}.",
                (int)response.StatusCode,
                body);
        }

        TokenResponse? token;
        try
        {
            token = JsonSerializer.Deserialize<TokenResponse>(body);
        }
        catch (JsonException ex)
        {
            throw new FreepbxRestException("Unable to parse the FreePBX token response.", (int)response.StatusCode, body + " " + ex.Message);
        }

        if (token is null || string.IsNullOrWhiteSpace(token.AccessToken))
        {
            throw new FreepbxRestException("FreePBX token response did not contain an access_token.", (int)response.StatusCode, body);
        }

        _accessToken = token.AccessToken;
        var lifetime = token.ExpiresIn > 0 ? token.ExpiresIn : 3600;
        _expiresAt = DateTimeOffset.UtcNow.AddSeconds(Math.Max(30, lifetime - 30));

        _logger?.LogDebug("Acquired FreePBX access token, expires at {ExpiresAt:o}", _expiresAt);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        if (_ownsHttpClient)
        {
            _httpClient.Dispose();
        }

        _gate.Dispose();
        _disposed = true;
    }

    private sealed class TokenResponse
    {
        [System.Text.Json.Serialization.JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("token_type")]
        public string? TokenType { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("refresh_token")]
        public string? RefreshToken { get; set; }
    }
}
