using Microsoft.Extensions.Logging;
using Sengsara.Freepbx.Abstractions.Interfaces;
using Sengsara.Freepbx.Abstractions.Interfaces.Services;
using Sengsara.Freepbx.Abstractions.Models;

namespace Sengsara.Freepbx.Services;

/// <summary>
/// Service for managing FreePBX ring groups through GraphQL.
/// </summary>
public sealed class RingGroupService : GraphQLServiceBase, IRingGroupService
{
    private const string GetAllQuery = @"
        query GetRingGroups {
          fetchAllRingGroups {
            totalCount
            ringgroups { " + GraphQLFields.RingGroup + @" }
          }
        }";

    private const string GetByIdQuery = @"
        query GetRingGroup($groupNumber: ID) {
          fetchRingGroup(groupNumber: $groupNumber) { " + GraphQLFields.RingGroup + @" }
        }";

    private const string CreateMutation = @"
        mutation AddRingGroup($input: addRingGroupInput!) {
          addRingGroup(input: $input) {
            status
            message
            response { " + GraphQLFields.RingGroup + @" }
          }
        }";

    private const string UpdateMutation = @"
        mutation UpdateRingGroup($input: updateRingGroupInput!) {
          updateRingGroup(input: $input) {
            status
            message
            response { " + GraphQLFields.RingGroup + @" }
          }
        }";

    private const string DeleteMutation = @"
        mutation DeleteRingGroup($input: DeleteRingGroupInput!) {
          deleteRingGroup(input: $input) { status message }
        }";

    /// <summary>
    /// Creates a new ring group service.
    /// </summary>
    public RingGroupService(IGraphQLExecutor graphQL, ILogger<RingGroupService> logger)
        : base(graphQL, logger)
    {
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<RingGroupDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await GraphQL.ExecuteQueryAsync<RingGroupsResponse>(GetAllQuery, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.FetchAllRingGroups?.RingGroups ?? [];
    }

    /// <inheritdoc />
    public async Task<RingGroupDto?> GetByIdAsync(string groupNumber, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(groupNumber);

        var result = await GraphQL.TryExecuteQueryAsync<RingGroupResponse>(
            GetByIdQuery,
            new { groupNumber },
            cancellationToken).ConfigureAwait(false);

        var group = result.Data?.FetchRingGroup;
        return group?.GroupNumber is null ? null : group;
    }

    /// <inheritdoc />
    public async Task<RingGroupDto?> CreateAsync(AddRingGroupRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = await GraphQL.ExecuteMutationAsync<RingGroupPayloadResponse>(
            CreateMutation,
            new { input = request },
            cancellationToken).ConfigureAwait(false);

        return response.AddRingGroup?.Response;
    }

    /// <inheritdoc />
    public async Task<RingGroupDto?> UpdateAsync(UpdateRingGroupRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = await GraphQL.ExecuteMutationAsync<RingGroupPayloadResponse>(
            UpdateMutation,
            new { input = request },
            cancellationToken).ConfigureAwait(false);

        return response.UpdateRingGroup?.Response;
    }

    /// <inheritdoc />
    public async Task<MutationResult> DeleteAsync(int groupNumber, CancellationToken cancellationToken = default)
    {
        var response = await GraphQL.ExecuteMutationAsync<DeleteRingGroupResponse>(
            DeleteMutation,
            new { input = new { groupNumber } },
            cancellationToken).ConfigureAwait(false);

        return ToResult(response.DeleteRingGroup);
    }

    private sealed class RingGroupsResponse
    {
        public RingGroupConnection? FetchAllRingGroups { get; set; }
    }

    private sealed class RingGroupConnection
    {
        public int? TotalCount { get; set; }

        public List<RingGroupDto>? RingGroups { get; set; }
    }

    private sealed class RingGroupResponse
    {
        public RingGroupDto? FetchRingGroup { get; set; }
    }

    private sealed class RingGroupPayloadResponse
    {
        public RingGroupPayload? AddRingGroup { get; set; }

        public RingGroupPayload? UpdateRingGroup { get; set; }
    }

    private sealed class RingGroupPayload : MutationPayload
    {
        public RingGroupDto? Response { get; set; }
    }

    private sealed class DeleteRingGroupResponse
    {
        public MutationPayload? DeleteRingGroup { get; set; }
    }
}
