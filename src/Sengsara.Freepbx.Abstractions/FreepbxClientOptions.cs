namespace Sengsara.Freepbx;

/// <summary>
/// Configuration options for connecting to a FreePBX 17 instance through its
/// GraphQL and REST APIs.
/// </summary>
/// <remarks>
/// FreePBX 17 exposes its API under <c>/admin/api/api</c> and protects it with an
/// OAuth2 resource server. Any of the following authentication modes can be used,
/// evaluated in order:
/// <list type="number">
///   <item><description>A pre-acquired bearer <see cref="AccessToken"/>.</description></item>
///   <item><description>The <c>client_credentials</c> grant using <see cref="ClientId"/> and <see cref="ClientSecret"/>.</description></item>
///   <item><description>The <c>password</c> grant using <see cref="Username"/> and <see cref="Password"/>.</description></item>
/// </list>
/// </remarks>
public class FreepbxClientOptions
{
    /// <summary>
    /// Root URL of the FreePBX instance, for example
    /// <c>https://pbx.example.com</c>. A base path is supported, for example
    /// <c>https://host/freepbx</c>.
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// Path (relative to <see cref="BaseUrl"/>) of the GraphQL endpoint.
    /// </summary>
    public string GraphQLPath { get; set; } = "/admin/api/api/gql";

    /// <summary>
    /// Path (relative to <see cref="BaseUrl"/>) of the REST API root.
    /// </summary>
    public string RestPath { get; set; } = "/admin/api/api/rest";

    /// <summary>
    /// Path (relative to <see cref="BaseUrl"/>) of the OAuth2 token endpoint.
    /// </summary>
    public string TokenPath { get; set; } = "/admin/api/api/token";

    /// <summary>
    /// OAuth2 client identifier used for the <c>client_credentials</c> grant.
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// OAuth2 client secret used for the <c>client_credentials</c> grant.
    /// </summary>
    public string? ClientSecret { get; set; }

    /// <summary>
    /// Username used for the <c>password</c> grant.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Password used for the <c>password</c> grant.
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// OAuth2 scopes to request. Defaults to <c>gql rest</c> which is required to
    /// use both the GraphQL and REST surfaces.
    /// </summary>
    public string Scope { get; set; } = "gql rest";

    /// <summary>
    /// Optional pre-acquired bearer token. When set, no token request is made
    /// until the token is known to be expired.
    /// </summary>
    public string? AccessToken { get; set; }

    /// <summary>
    /// Expiry of <see cref="AccessToken"/>, when known.
    /// </summary>
    public DateTimeOffset? AccessTokenExpiresAt { get; set; }

    /// <summary>
    /// HTTP request timeout in seconds. Defaults to 30.
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// Enables retrying transient failures (network errors and 5xx responses).
    /// Defaults to <c>true</c>.
    /// </summary>
    public bool EnableRetry { get; set; } = true;

    /// <summary>
    /// Maximum number of retry attempts when <see cref="EnableRetry"/> is enabled.
    /// Defaults to 3.
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 3;

    /// <summary>
    /// Allows connecting to a FreePBX instance using a self-signed TLS
    /// certificate. Do not enable in production.
    /// </summary>
    public bool AllowInsecureCertificates { get; set; }

    /// <summary>
    /// Optional HTTP user agent.
    /// </summary>
    public string UserAgent { get; set; } = "Sengsara.Freepbx";

    /// <summary>
    /// Creates an empty options instance.
    /// </summary>
    public FreepbxClientOptions()
    {
    }

    /// <summary>
    /// Creates an options instance for the specified base URL.
    /// </summary>
    /// <param name="baseUrl">Root URL of the FreePBX instance.</param>
    public FreepbxClientOptions(string baseUrl)
    {
        BaseUrl = baseUrl;
    }

    /// <summary>
    /// Absolute URI of the GraphQL endpoint.
    /// </summary>
    public Uri GraphQLUri => BuildUri(GraphQLPath);

    /// <summary>
    /// Absolute URI of the REST API root.
    /// </summary>
    public Uri RestBaseUri => BuildUri(RestPath);

    /// <summary>
    /// Absolute URI of the OAuth2 token endpoint.
    /// </summary>
    public Uri TokenUri => BuildUri(TokenPath);

    /// <summary>
    /// Builds an absolute URI by appending <paramref name="path"/> to
    /// <see cref="BaseUrl"/>, preserving any base path.
    /// </summary>
    public Uri BuildUri(string path)
    {
        if (string.IsNullOrWhiteSpace(BaseUrl))
        {
            throw new InvalidOperationException("FreepbxClientOptions.BaseUrl is required.");
        }

        var combined = BaseUrl.TrimEnd('/') + "/" + path.TrimStart('/');
        return new Uri(combined, UriKind.Absolute);
    }

    /// <summary>
    /// True when an OAuth2 token can be acquired from the configured credentials.
    /// </summary>
    public bool CanAcquireToken =>
        (HasValue(ClientId) && HasValue(ClientSecret)) ||
        (HasValue(Username) && HasValue(Password));

    /// <summary>
    /// Validates the options and throws when required values are missing.
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(BaseUrl))
        {
            throw new ArgumentException("FreepbxClientOptions.BaseUrl is required.", nameof(BaseUrl));
        }

        if (!Uri.TryCreate(BaseUrl, UriKind.Absolute, out _))
        {
            throw new ArgumentException($"FreepbxClientOptions.BaseUrl '{BaseUrl}' is not a valid absolute URL.", nameof(BaseUrl));
        }

        if (TimeoutSeconds <= 0)
        {
            throw new ArgumentException("TimeoutSeconds must be greater than zero.", nameof(TimeoutSeconds));
        }

        if (MaxRetryAttempts < 0)
        {
            throw new ArgumentException("MaxRetryAttempts cannot be negative.", nameof(MaxRetryAttempts));
        }

        if (!CanAcquireToken && string.IsNullOrWhiteSpace(AccessToken))
        {
            throw new ArgumentException(
                "No authentication configured. Provide AccessToken, ClientId/ClientSecret, or Username/Password.");
        }
    }

    private static bool HasValue(string? value) => !string.IsNullOrWhiteSpace(value);
}
