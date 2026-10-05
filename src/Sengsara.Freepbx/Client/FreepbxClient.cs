using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sengsara.Freepbx.Abstractions.Interfaces;
using Sengsara.Freepbx.Abstractions.Interfaces.Services;
using Sengsara.Freepbx.Authentication;
using Sengsara.Freepbx.GraphQL;
using Sengsara.Freepbx.Http;
using Sengsara.Freepbx.Rest;
using Sengsara.Freepbx.Services;

namespace Sengsara.Freepbx.Client;

/// <summary>
/// Default implementation of <see cref="IFreepbxClient"/>.
/// </summary>
public sealed class FreepbxClient : IFreepbxClient
{
    private readonly HttpClient _graphQLHttpClient;
    private readonly HttpClient _restHttpClient;
    private readonly FreepbxTokenProvider _tokenProvider;
    private readonly ILogger<FreepbxClient> _logger;
    private bool _disposed;

    /// <inheritdoc />
    public FreepbxClientOptions Options { get; }

    /// <inheritdoc />
    public IGraphQLExecutor GraphQL { get; }

    /// <inheritdoc />
    public IExtensionService Extensions { get; }

    /// <inheritdoc />
    public ICoreUserService CoreUsers { get; }

    /// <inheritdoc />
    public ICoreDeviceService CoreDevices { get; }

    /// <inheritdoc />
    public IRingGroupService RingGroups { get; }

    /// <inheritdoc />
    public IInboundRouteService InboundRoutes { get; }

    /// <inheritdoc />
    public IRecordingService Recordings { get; }

    /// <inheritdoc />
    public IMusicOnHoldService MusicOnHold { get; }

    /// <inheritdoc />
    public IVoiceMailService VoiceMail { get; }

    /// <inheritdoc />
    public IFollowMeService FollowMe { get; }

    /// <inheritdoc />
    public ICdrService Cdrs { get; }

    /// <inheritdoc />
    public IQueueService Queues { get; }

    /// <summary>
    /// Creates a new FreePBX client.
    /// </summary>
    /// <param name="options">Client options.</param>
    /// <param name="loggerFactory">Optional logger factory.</param>
    public FreepbxClient(FreepbxClientOptions options, ILoggerFactory? loggerFactory = null)
    {
        Options = options ?? throw new ArgumentNullException(nameof(options));
        Options.Validate();

        var factory = loggerFactory ?? NullLoggerFactory.Instance;
        _logger = factory.CreateLogger<FreepbxClient>();

        _tokenProvider = new FreepbxTokenProvider(options, logger: factory.CreateLogger<FreepbxTokenProvider>());

        _graphQLHttpClient = new HttpClient(new FreepbxAuthHandler(_tokenProvider, FreepbxHttpClientFactory.CreateHandler(options)));
        _restHttpClient = new HttpClient(new FreepbxAuthHandler(_tokenProvider, FreepbxHttpClientFactory.CreateHandler(options)));
        FreepbxHttpClientFactory.Apply(options, _graphQLHttpClient);
        FreepbxHttpClientFactory.Apply(options, _restHttpClient);

        GraphQL = new GraphQLExecutor(_graphQLHttpClient, options, factory.CreateLogger<GraphQLExecutor>());

        Extensions = new ExtensionService(GraphQL, factory.CreateLogger<ExtensionService>());
        CoreUsers = new CoreUserService(GraphQL, factory.CreateLogger<CoreUserService>());
        CoreDevices = new CoreDeviceService(GraphQL, factory.CreateLogger<CoreDeviceService>());
        RingGroups = new RingGroupService(GraphQL, factory.CreateLogger<RingGroupService>());
        InboundRoutes = new InboundRouteService(GraphQL, factory.CreateLogger<InboundRouteService>());
        Recordings = new RecordingService(GraphQL, factory.CreateLogger<RecordingService>());
        MusicOnHold = new MusicOnHoldService(GraphQL, factory.CreateLogger<MusicOnHoldService>());
        VoiceMail = new VoiceMailService(GraphQL, factory.CreateLogger<VoiceMailService>());
        FollowMe = new FollowMeService(GraphQL, factory.CreateLogger<FollowMeService>());
        Cdrs = new CdrService(GraphQL, factory.CreateLogger<CdrService>());

        var restClient = new FreepbxRestClient(_restHttpClient, options);
        Queues = new QueueService(restClient, factory.CreateLogger<QueueService>());
    }

    /// <inheritdoc />
    public async Task<bool> TestConnectionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await GraphQL.ExecuteRawAsync("query { __typename }", cancellationToken: cancellationToken).ConfigureAwait(false);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "FreePBX connectivity test failed.");
            return false;
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _graphQLHttpClient.Dispose();
        _restHttpClient.Dispose();
        _tokenProvider.Dispose();
        _disposed = true;
    }
}
