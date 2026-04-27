using Microsoft.Extensions.Logging;
using Sengsara.Freepbx.Abstractions.Interfaces.Services;
using Sengsara.Freepbx.Abstractions.Models;
using Sengsara.Freepbx.Client;

namespace Sengsara.Freepbx.Services;

/// <summary>
/// Service for managing FreePBX queues
/// </summary>
public class QueueService : IQueueService
{
    private readonly FreepbxClient _client;
    private readonly ILogger<QueueService>? _logger;

    private const string GetAllQuery = @"
        query GetQueues {
            queues {
                id
                extension
                description
                password
                maxMembers
                timeout
                strategy
                weight
                wrapupTime
                autofill
                musicOnHold
                announce
                announceFrequency
                queueWaitTimeAnnounce
                queueLengthAnnounce
                createDate
                modifyDate
                enabled
            }
        }";

    private const string GetByIdQuery = @"
        query GetQueue($id: ID!) {
            queue(id: $id) {
                id
                extension
                description
                password
                maxMembers
                timeout
                strategy
                weight
                wrapupTime
                autofill
                musicOnHold
                announce
                announceFrequency
                queueWaitTimeAnnounce
                queueLengthAnnounce
                createDate
                modifyDate
                enabled
            }
        }";

    private const string GetMembersQuery = @"
        query GetQueueMembers($queueId: ID!) {
            queueMembers(queueId: $queueId) {
                id
                queueId
                extension
                name
                penalty
                paused
                stateInterface
                memberName
                status
                callsTaken
                lastCall
                wrapUpTime
            }
        }";

    private const string CreateMutation = @"
        mutation CreateQueue($input: CreateQueueInput!) {
            createQueue(input: $input) {
                id
                extension
                description
                password
                maxMembers
                timeout
                strategy
                weight
                wrapupTime
                autofill
                musicOnHold
                announce
                announceFrequency
                queueWaitTimeAnnounce
                queueLengthAnnounce
                createDate
                modifyDate
                enabled
            }
        }";

    private const string UpdateMutation = @"
        mutation UpdateQueue($id: ID!, $input: UpdateQueueInput!) {
            updateQueue(id: $id, input: $input) {
                id
                extension
                description
                password
                maxMembers
                timeout
                strategy
                weight
                wrapupTime
                autofill
                musicOnHold
                announce
                announceFrequency
                queueWaitTimeAnnounce
                queueLengthAnnounce
                createDate
                modifyDate
                enabled
            }
        }";

    private const string DeleteMutation = @"
        mutation DeleteQueue($id: ID!) {
            deleteQueue(id: $id)
        }";

    private const string AddMemberMutation = @"
        mutation AddQueueMember($queueId: ID!, $input: AddQueueMemberInput!) {
            addQueueMember(queueId: $queueId, input: $input)
        }";

    private const string RemoveMemberMutation = @"
        mutation RemoveQueueMember($queueId: ID!, $memberExtension: String!) {
            removeQueueMember(queueId: $queueId, memberExtension: $memberExtension)
        }";

    public QueueService(FreepbxClient client, ILogger<QueueService>? logger = null)
    {
        _client = client;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<QueueDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation("Retrieving all queues");

        var response = await _client.GraphQL.ExecuteQueryAsync<QueuesResponse>(
            GetAllQuery,
            cancellationToken: cancellationToken);

        return response.Queues ?? [];
    }

    /// <inheritdoc />
    public async Task<QueueDto?> GetByIdAsync(string queueId, CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation("Retrieving queue with ID: {QueueId}", queueId);

        var response = await _client.GraphQL.ExecuteQueryAsync<QueueResponse>(
            GetByIdQuery,
            new Dictionary<string, object?> { ["id"] = queueId },
            cancellationToken);

        return response.Queue;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<QueueMemberDto>> GetMembersAsync(string queueId, CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation("Retrieving members for queue: {QueueId}", queueId);

        var response = await _client.GraphQL.ExecuteQueryAsync<QueueMembersResponse>(
            GetMembersQuery,
            new Dictionary<string, object?> { ["queueId"] = queueId },
            cancellationToken);

        return response.QueueMembers ?? [];
    }

    /// <inheritdoc />
    public async Task<QueueDto> CreateAsync(CreateQueueRequest request, CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation("Creating queue: {Extension}", request.Extension);

        var input = new Dictionary<string, object?>
        {
            ["extension"] = request.Extension,
            ["description"] = request.Description,
            ["password"] = request.Password,
            ["maxMembers"] = request.MaxMembers,
            ["timeout"] = request.Timeout,
            ["strategy"] = request.Strategy,
            ["weight"] = request.Weight,
            ["wrapupTime"] = request.WrapupTime,
            ["autofill"] = request.Autofill,
            ["musicOnHold"] = request.MusicOnHold,
            ["announce"] = request.Announce,
            ["announceFrequency"] = request.AnnounceFrequency,
            ["queueWaitTimeAnnounce"] = request.QueueWaitTimeAnnounce,
            ["queueLengthAnnounce"] = request.QueueLengthAnnounce
        };

        var response = await _client.GraphQL.ExecuteMutationAsync<CreateQueueResponse>(
            CreateMutation,
            new Dictionary<string, object?> { ["input"] = input },
            cancellationToken);

        _logger?.LogInformation("Queue created with ID: {QueueId}", response.CreateQueue.Id);

        return response.CreateQueue;
    }

    /// <inheritdoc />
    public async Task<QueueDto> UpdateAsync(string queueId, UpdateQueueRequest request, CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation("Updating queue: {QueueId}", queueId);

        var input = new Dictionary<string, object?>();

        if (request.Description != null) input["description"] = request.Description;
        if (request.Password != null) input["password"] = request.Password;
        if (request.MaxMembers != null) input["maxMembers"] = request.MaxMembers;
        if (request.Timeout != null) input["timeout"] = request.Timeout;
        if (request.Strategy != null) input["strategy"] = request.Strategy;
        if (request.Weight != null) input["weight"] = request.Weight;
        if (request.WrapupTime != null) input["wrapupTime"] = request.WrapupTime;
        if (request.Autofill != null) input["autofill"] = request.Autofill;
        if (request.MusicOnHold != null) input["musicOnHold"] = request.MusicOnHold;
        if (request.Announce != null) input["announce"] = request.Announce;
        if (request.AnnounceFrequency != null) input["announceFrequency"] = request.AnnounceFrequency;
        if (request.QueueWaitTimeAnnounce != null) input["queueWaitTimeAnnounce"] = request.QueueWaitTimeAnnounce;
        if (request.QueueLengthAnnounce != null) input["queueLengthAnnounce"] = request.QueueLengthAnnounce;

        var response = await _client.GraphQL.ExecuteMutationAsync<UpdateQueueResponse>(
            UpdateMutation,
            new Dictionary<string, object?>
            {
                ["id"] = queueId,
                ["input"] = input
            },
            cancellationToken);

        _logger?.LogInformation("Queue updated: {QueueId}", queueId);

        return response.UpdateQueue;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(string queueId, CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation("Deleting queue: {QueueId}", queueId);

        var response = await _client.GraphQL.ExecuteMutationAsync<DeleteQueueResponse>(
            DeleteMutation,
            new Dictionary<string, object?> { ["id"] = queueId },
            cancellationToken);

        _logger?.LogInformation("Queue deleted: {QueueId}, Result: {Result}", queueId, response.DeleteQueue);

        return response.DeleteQueue;
    }

    /// <inheritdoc />
    public async Task<bool> AddMemberAsync(string queueId, AddQueueMemberRequest request, CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation("Adding member {Extension} to queue {QueueId}", request.Extension, queueId);

        var input = new Dictionary<string, object?>
        {
            ["extension"] = request.Extension,
            ["penalty"] = request.Penalty,
            ["paused"] = request.Paused,
            ["stateInterface"] = request.StateInterface
        };

        var response = await _client.GraphQL.ExecuteMutationAsync<AddMemberResponse>(
            AddMemberMutation,
            new Dictionary<string, object?>
            {
                ["queueId"] = queueId,
                ["input"] = input
            },
            cancellationToken);

        _logger?.LogInformation("Member added: {Result}", response.AddQueueMember);

        return response.AddQueueMember;
    }

    /// <inheritdoc />
    public async Task<bool> RemoveMemberAsync(string queueId, string memberExtension, CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation("Removing member {Extension} from queue {QueueId}", memberExtension, queueId);

        var response = await _client.GraphQL.ExecuteMutationAsync<RemoveMemberResponse>(
            RemoveMemberMutation,
            new Dictionary<string, object?>
            {
                ["queueId"] = queueId,
                ["memberExtension"] = memberExtension
            },
            cancellationToken);

        _logger?.LogInformation("Member removed: {Result}", response.RemoveQueueMember);

        return response.RemoveQueueMember;
    }

    // Response DTOs
    private class QueuesResponse
    {
        public List<QueueDto>? Queues { get; set; }
    }

    private class QueueResponse
    {
        public QueueDto? Queue { get; set; }
    }

    private class QueueMembersResponse
    {
        public List<QueueMemberDto>? QueueMembers { get; set; }
    }

    private class CreateQueueResponse
    {
        public QueueDto CreateQueue { get; set; } = new();
    }

    private class UpdateQueueResponse
    {
        public QueueDto UpdateQueue { get; set; } = new();
    }

    private class DeleteQueueResponse
    {
        public bool DeleteQueue { get; set; }
    }

    private class AddMemberResponse
    {
        public bool AddQueueMember { get; set; }
    }

    private class RemoveMemberResponse
    {
        public bool RemoveQueueMember { get; set; }
    }
}