using Sengsara.Freepbx.Abstractions.Interfaces.Services;

namespace Sengsara.Freepbx.Abstractions.Interfaces;

/// <summary>
/// Main entry point for interacting with a FreePBX 17 instance.
/// </summary>
public interface IFreepbxClient : IDisposable
{
    /// <summary>The options used to create the client.</summary>
    FreepbxClientOptions Options { get; }

    /// <summary>Direct access to the GraphQL executor.</summary>
    IGraphQLExecutor GraphQL { get; }

    /// <summary>Extension (Core) service.</summary>
    IExtensionService Extensions { get; }

    /// <summary>Core user service.</summary>
    ICoreUserService CoreUsers { get; }

    /// <summary>Core device service.</summary>
    ICoreDeviceService CoreDevices { get; }

    /// <summary>Ring group service.</summary>
    IRingGroupService RingGroups { get; }

    /// <summary>Inbound route (DID) service.</summary>
    IInboundRouteService InboundRoutes { get; }

    /// <summary>System recordings service.</summary>
    IRecordingService Recordings { get; }

    /// <summary>Music on hold service.</summary>
    IMusicOnHoldService MusicOnHold { get; }

    /// <summary>Voicemail service.</summary>
    IVoiceMailService VoiceMail { get; }

    /// <summary>Follow me service.</summary>
    IFollowMeService FollowMe { get; }

    /// <summary>Call detail records service.</summary>
    ICdrService Cdrs { get; }

    /// <summary>Queue service (implemented over the REST API).</summary>
    IQueueService Queues { get; }

    /// <summary>
    /// Verifies connectivity and authentication by issuing a lightweight query.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True when the server responded successfully.</returns>
    Task<bool> TestConnectionAsync(CancellationToken cancellationToken = default);
}
