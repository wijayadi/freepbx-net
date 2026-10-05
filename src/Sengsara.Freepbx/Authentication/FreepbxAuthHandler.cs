using System.Net;
using System.Net.Http.Headers;
using Sengsara.Freepbx.Abstractions.Interfaces;

namespace Sengsara.Freepbx.Authentication;

/// <summary>
/// Adds a FreePBX OAuth2 bearer token to outgoing requests and transparently
/// refreshes it once when the server responds with <c>401 Unauthorized</c>.
/// </summary>
public sealed class FreepbxAuthHandler : DelegatingHandler
{
    private readonly ITokenProvider _tokenProvider;

    /// <summary>
    /// Creates a new authentication handler.
    /// </summary>
    /// <param name="tokenProvider">Token provider.</param>
    /// <param name="innerHandler">Inner handler.</param>
    public FreepbxAuthHandler(ITokenProvider tokenProvider, HttpMessageHandler innerHandler)
        : base(innerHandler)
    {
        _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
    }

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        byte[]? body = null;
        if (request.Content is not null)
        {
            body = await request.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
        }

        var attempt = 0;
        while (true)
        {
            using var message = CloneRequest(request, body);
            var token = await _tokenProvider.GetAccessTokenAsync(forceRefresh: attempt > 0, cancellationToken).ConfigureAwait(false);
            message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await base.SendAsync(message, cancellationToken).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.Unauthorized && attempt == 0)
            {
                response.Dispose();
                _tokenProvider.Invalidate();
                attempt++;
                continue;
            }

            return response;
        }
    }

    private static HttpRequestMessage CloneRequest(HttpRequestMessage request, byte[]? body)
    {
        var clone = new HttpRequestMessage(request.Method, request.RequestUri)
        {
            Version = request.Version,
            VersionPolicy = request.VersionPolicy
        };

        foreach (var header in request.Headers)
        {
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        if (body is not null)
        {
            clone.Content = new ByteArrayContent(body);
            if (request.Content is not null)
            {
                foreach (var header in request.Content.Headers)
                {
                    clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }
        }

        return clone;
    }
}
