using Sengsara.Freepbx.Abstractions.Models;

namespace Sengsara.Freepbx.Abstractions.Interfaces.Services;

/// <summary>
/// Service for managing FreePBX queues
/// </summary>
public interface IQueueService
{
    /// <summary>
    /// Gets all queues
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of queues</returns>
    Task<IReadOnlyList<QueueDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a queue by ID
    /// </summary>
    /// <param name="queueId">Queue ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Queue details</returns>
    Task<QueueDto?> GetByIdAsync(string queueId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all members of a queue
    /// </summary>
    /// <param name="queueId">Queue ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of queue members</returns>
    Task<IReadOnlyList<QueueMemberDto>> GetMembersAsync(string queueId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new queue
    /// </summary>
    /// <param name="request">Create queue request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created queue</returns>
    Task<QueueDto> CreateAsync(CreateQueueRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing queue
    /// </summary>
    /// <param name="queueId">Queue ID</param>
    /// <param name="request">Update queue request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated queue</returns>
    Task<QueueDto> UpdateAsync(string queueId, UpdateQueueRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a queue
    /// </summary>
    /// <param name="queueId">Queue ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if deletion was successful</returns>
    Task<bool> DeleteAsync(string queueId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a member to a queue
    /// </summary>
    /// <param name="queueId">Queue ID</param>
    /// <param name="request">Add member request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if member was added successfully</returns>
    Task<bool> AddMemberAsync(string queueId, AddQueueMemberRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a member from a queue
    /// </summary>
    /// <param name="queueId">Queue ID</param>
    /// <param name="memberExtension">Member's extension number</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if member was removed successfully</returns>
    Task<bool> RemoveMemberAsync(string queueId, string memberExtension, CancellationToken cancellationToken = default);
}

/// <summary>
/// Request model for creating a queue
/// </summary>
public class CreateQueueRequest
{
    public required string Extension { get; set; }
    public required string Description { get; set; }
    public string? Password { get; set; }
    public int? MaxMembers { get; set; }
    public int? Timeout { get; set; }
    public string? Strategy { get; set; }
    public int? Weight { get; set; }
    public int? WrapupTime { get; set; }
    public bool? Autofill { get; set; }
    public string? MusicOnHold { get; set; }
    public string? Announce { get; set; }
    public string? AnnounceFrequency { get; set; }
    public string? QueueWaitTimeAnnounce { get; set; }
    public string? QueueLengthAnnounce { get; set; }
}

/// <summary>
/// Request model for updating a queue
/// </summary>
public class UpdateQueueRequest
{
    public string? Description { get; set; }
    public string? Password { get; set; }
    public int? MaxMembers { get; set; }
    public int? Timeout { get; set; }
    public string? Strategy { get; set; }
    public int? Weight { get; set; }
    public int? WrapupTime { get; set; }
    public bool? Autofill { get; set; }
    public string? MusicOnHold { get; set; }
    public string? Announce { get; set; }
    public string? AnnounceFrequency { get; set; }
    public string? QueueWaitTimeAnnounce { get; set; }
    public string? QueueLengthAnnounce { get; set; }
}

/// <summary>
/// Request model for adding a member to a queue
/// </summary>
public class AddQueueMemberRequest
{
    public required string Extension { get; set; }
    public string? Penalty { get; set; }
    public bool? Paused { get; set; }
    public string? StateInterface { get; set; }
}