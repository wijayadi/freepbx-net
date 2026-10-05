using System.Net.Http.Headers;

namespace Sengsara.Freepbx.Http;

/// <summary>
/// Creates configured <see cref="HttpClient"/> instances for talking to FreePBX.
/// </summary>
public static class FreepbxHttpClientFactory
{
    /// <summary>
    /// Creates an <see cref="HttpClientHandler"/> honoring the TLS options.
    /// </summary>
    public static HttpClientHandler CreateHandler(FreepbxClientOptions options)
    {
        var handler = new HttpClientHandler();
        if (options.AllowInsecureCertificates)
        {
            handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
        }

        return handler;
    }

    /// <summary>
    /// Creates a standalone <see cref="HttpClient"/> (without the auth handler).
    /// </summary>
    public static HttpClient CreateClient(FreepbxClientOptions options)
    {
        var client = new HttpClient(CreateHandler(options));
        Apply(options, client);
        return client;
    }

    /// <summary>
    /// Applies standard settings to an existing <see cref="HttpClient"/>.
    /// </summary>
    public static void Apply(FreepbxClientOptions options, HttpClient client)
    {
        client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds <= 0 ? 30 : options.TimeoutSeconds);
        if (!string.IsNullOrWhiteSpace(options.UserAgent))
        {
            client.DefaultRequestHeaders.UserAgent.Clear();
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Sengsara.Freepbx", "1.0"));
        }
    }
}
