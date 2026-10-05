using Sengsara.Freepbx.Abstractions.Models;

namespace Sengsara.Freepbx.Abstractions.Interfaces.Services;

/// <summary>
/// Service for managing FreePBX ring groups.
/// </summary>
public interface IRingGroupService
{
    /// <summary>Gets all ring groups.</summary>
    Task<IReadOnlyList<RingGroupDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a ring group by group number.</summary>
    Task<RingGroupDto?> GetByIdAsync(string groupNumber, CancellationToken cancellationToken = default);

    /// <summary>Creates a ring group.</summary>
    Task<RingGroupDto?> CreateAsync(AddRingGroupRequest request, CancellationToken cancellationToken = default);

    /// <summary>Updates a ring group.</summary>
    Task<RingGroupDto?> UpdateAsync(UpdateRingGroupRequest request, CancellationToken cancellationToken = default);

    /// <summary>Deletes a ring group.</summary>
    Task<MutationResult> DeleteAsync(int groupNumber, CancellationToken cancellationToken = default);
}

/// <summary>
/// Service for managing FreePBX inbound routes (DIDs).
/// </summary>
public interface IInboundRouteService
{
    /// <summary>Gets all inbound routes.</summary>
    Task<IReadOnlyList<InboundRouteDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets an inbound route by id.</summary>
    Task<InboundRouteDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>Creates an inbound route.</summary>
    Task<InboundRouteDto?> CreateAsync(AddInboundRouteRequest request, CancellationToken cancellationToken = default);

    /// <summary>Updates an inbound route.</summary>
    Task<InboundRouteDto?> UpdateAsync(UpdateInboundRouteRequest request, CancellationToken cancellationToken = default);

    /// <summary>Removes an inbound route.</summary>
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

/// <summary>
/// Service for managing FreePBX system recordings.
/// </summary>
public interface IRecordingService
{
    /// <summary>Gets all recordings.</summary>
    Task<IReadOnlyList<RecordingDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets recordings files, optionally filtered by a search term.</summary>
    Task<IReadOnlyList<string>> GetFilesAsync(string? search = null, CancellationToken cancellationToken = default);

    /// <summary>Creates or updates a recording.</summary>
    Task<MutationResult> SaveAsync(SaveRecordingRequest request, bool create, CancellationToken cancellationToken = default);

    /// <summary>Deletes a recording.</summary>
    Task<MutationResult> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

/// <summary>
/// Service for reading FreePBX music on hold classes.
/// </summary>
public interface IMusicOnHoldService
{
    /// <summary>Gets all music on hold classes.</summary>
    Task<IReadOnlyList<MusicOnHoldDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a music on hold class by id.</summary>
    Task<MusicOnHoldDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
}

/// <summary>
/// Service for managing FreePBX voicemail mailboxes.
/// </summary>
public interface IVoiceMailService
{
    /// <summary>Gets the voicemail configuration for an extension.</summary>
    Task<VoiceMailDto?> GetAsync(string extensionId, CancellationToken cancellationToken = default);

    /// <summary>Enables voicemail for an extension.</summary>
    Task<MutationResult> EnableAsync(EnableVoiceMailRequest request, CancellationToken cancellationToken = default);

    /// <summary>Disables voicemail for an extension.</summary>
    Task<MutationResult> DisableAsync(string extensionId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Service for managing FreePBX follow me configuration.
/// </summary>
public interface IFollowMeService
{
    /// <summary>Gets the follow me configuration for an extension.</summary>
    Task<FollowMeDto?> GetAsync(string extensionId, CancellationToken cancellationToken = default);

    /// <summary>Updates the follow me configuration for an extension.</summary>
    Task<MutationResult> UpdateAsync(UpdateFollowMeRequest request, CancellationToken cancellationToken = default);

    /// <summary>Enables follow me for an extension.</summary>
    Task<MutationResult> EnableAsync(string extensionId, CancellationToken cancellationToken = default);

    /// <summary>Disables follow me for an extension.</summary>
    Task<MutationResult> DisableAsync(string extensionId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Service for querying FreePBX call detail records.
/// </summary>
public interface ICdrService
{
    /// <summary>
    /// Gets call detail records.
    /// </summary>
    /// <param name="startDate">Optional start date (inclusive).</param>
    /// <param name="endDate">Optional end date (inclusive).</param>
    /// <param name="first">Maximum number of records to return.</param>
    /// <param name="orderBy">Sort order.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<IReadOnlyList<CdrDto>> GetAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        int? first = null,
        CdrOrderBy orderBy = CdrOrderBy.Date,
        CancellationToken cancellationToken = default);

    /// <summary>Gets a single call detail record by id.</summary>
    Task<CdrDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
}

/// <summary>
/// Service for FreePBX queues. Queues are not exposed through GraphQL, so this
/// service uses the REST API.
/// </summary>
public interface IQueueService
{
    /// <summary>Gets all queues (number and name).</summary>
    Task<IReadOnlyList<QueueDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a single queue including its settings.</summary>
    Task<QueueDto?> GetByIdAsync(string queueExtension, CancellationToken cancellationToken = default);

    /// <summary>Gets the static and dynamic members for a single queue.</summary>
    Task<QueueMembers> GetMembersAsync(string queueExtension, CancellationToken cancellationToken = default);

    /// <summary>Gets the members for all queues keyed by queue number.</summary>
    Task<IReadOnlyDictionary<string, QueueMembers>> GetAllMembersAsync(CancellationToken cancellationToken = default);

    /// <summary>Replaces the static and dynamic members of a queue.</summary>
    Task<bool> SetMembersAsync(string queueExtension, QueueMembers members, CancellationToken cancellationToken = default);

    /// <summary>Adds a single member to a queue.</summary>
    Task<bool> AddMemberAsync(string queueExtension, string member, bool dynamic = false, int penalty = 0, CancellationToken cancellationToken = default);

    /// <summary>Removes a single member from a queue.</summary>
    Task<bool> RemoveMemberAsync(string queueExtension, string member, bool dynamic = false, CancellationToken cancellationToken = default);
}
