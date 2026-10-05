using Microsoft.Extensions.Logging;
using Sengsara.Freepbx.Abstractions.Interfaces;
using Sengsara.Freepbx.Abstractions.Interfaces.Services;
using Sengsara.Freepbx.Abstractions.Models;

namespace Sengsara.Freepbx.Services;

/// <summary>
/// Service for reading FreePBX music on hold classes through GraphQL.
/// </summary>
public sealed class MusicOnHoldService : GraphQLServiceBase, IMusicOnHoldService
{
    private const string GetAllQuery = @"
        query GetMusicOnHold {
          allMusiconholds {
            totalCount
            musiconholds { " + GraphQLFields.MusicOnHold + @" }
          }
        }";

    private const string GetByIdQuery = @"
        query GetMusicOnHoldById($id: ID) {
          musiconhold(id: $id) { " + GraphQLFields.MusicOnHold + @" }
        }";

    /// <summary>
    /// Creates a new music on hold service.
    /// </summary>
    public MusicOnHoldService(IGraphQLExecutor graphQL, ILogger<MusicOnHoldService> logger)
        : base(graphQL, logger)
    {
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<MusicOnHoldDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await GraphQL.ExecuteQueryAsync<MusicOnHoldResponse>(GetAllQuery, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.AllMusiconholds?.Musiconholds ?? [];
    }

    /// <inheritdoc />
    public async Task<MusicOnHoldDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        var result = await GraphQL.TryExecuteQueryAsync<MusicOnHoldByIdResponse>(
            GetByIdQuery,
            new { id },
            cancellationToken).ConfigureAwait(false);

        var item = result.Data?.Musiconhold;
        return string.IsNullOrEmpty(item?.Category) ? null : item;
    }

    private sealed class MusicOnHoldResponse
    {
        public MusicOnHoldConnection? AllMusiconholds { get; set; }
    }

    private sealed class MusicOnHoldConnection
    {
        public int? TotalCount { get; set; }

        public List<MusicOnHoldDto>? Musiconholds { get; set; }
    }

    private sealed class MusicOnHoldByIdResponse
    {
        public MusicOnHoldDto? Musiconhold { get; set; }
    }
}

/// <summary>
/// Service for managing FreePBX voicemail mailboxes through GraphQL.
/// </summary>
public sealed class VoiceMailService : GraphQLServiceBase, IVoiceMailService
{
    private const string GetQuery = @"
        query GetVoiceMail($extensionId: ID!) {
          fetchVoiceMail(extensionId: $extensionId) { " + GraphQLFields.VoiceMail + @" }
        }";

    private const string EnableMutation = @"
        mutation EnableVoiceMail($input: enableVoiceMailInput!) {
          enableVoiceMail(input: $input) { status message }
        }";

    private const string DisableMutation = @"
        mutation DisableVoiceMail($input: disableVoiceMailInput!) {
          disableVoiceMail(input: $input) { status message }
        }";

    /// <summary>
    /// Creates a new voicemail service.
    /// </summary>
    public VoiceMailService(IGraphQLExecutor graphQL, ILogger<VoiceMailService> logger)
        : base(graphQL, logger)
    {
    }

    /// <inheritdoc />
    public async Task<VoiceMailDto?> GetAsync(string extensionId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(extensionId);

        var result = await GraphQL.TryExecuteQueryAsync<VoiceMailResponse>(
            GetQuery,
            new { extensionId },
            cancellationToken).ConfigureAwait(false);

        var voiceMail = result.Data?.FetchVoiceMail;
        return string.IsNullOrEmpty(voiceMail?.Context) ? null : voiceMail;
    }

    /// <inheritdoc />
    public async Task<MutationResult> EnableAsync(EnableVoiceMailRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = await GraphQL.ExecuteMutationAsync<EnableVoiceMailResponse>(
            EnableMutation,
            new { input = request },
            cancellationToken).ConfigureAwait(false);

        return ToResult(response.EnableVoiceMail);
    }

    /// <inheritdoc />
    public async Task<MutationResult> DisableAsync(string extensionId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(extensionId);

        var response = await GraphQL.ExecuteMutationAsync<DisableVoiceMailResponse>(
            DisableMutation,
            new { input = new { extensionId } },
            cancellationToken).ConfigureAwait(false);

        return ToResult(response.DisableVoiceMail);
    }

    private sealed class VoiceMailResponse
    {
        public VoiceMailDto? FetchVoiceMail { get; set; }
    }

    private sealed class EnableVoiceMailResponse
    {
        public MutationPayload? EnableVoiceMail { get; set; }
    }

    private sealed class DisableVoiceMailResponse
    {
        public MutationPayload? DisableVoiceMail { get; set; }
    }
}

/// <summary>
/// Service for managing FreePBX follow me configuration through GraphQL.
/// </summary>
public sealed class FollowMeService : GraphQLServiceBase, IFollowMeService
{
    private const string GetQuery = @"
        query GetFollowMe($extensionId: ID!) {
          fetchFollowMe(extensionId: $extensionId) { " + GraphQLFields.FollowMe + @" }
        }";

    private const string UpdateMutation = @"
        mutation UpdateFollowMe($input: updateFollowMeInput!) {
          updateFollowMe(input: $input) { status message }
        }";

    private const string EnableMutation = @"
        mutation EnableFollowMe($input: enableFollowMeInput!) {
          enableFollowMe(input: $input) { status message }
        }";

    private const string DisableMutation = @"
        mutation DisableFollowMe($input: disableFollowMeInput!) {
          disableFollowMe(input: $input) { status message }
        }";

    /// <summary>
    /// Creates a new follow me service.
    /// </summary>
    public FollowMeService(IGraphQLExecutor graphQL, ILogger<FollowMeService> logger)
        : base(graphQL, logger)
    {
    }

    /// <inheritdoc />
    public async Task<FollowMeDto?> GetAsync(string extensionId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(extensionId);

        var result = await GraphQL.TryExecuteQueryAsync<FollowMeResponse>(
            GetQuery,
            new { extensionId },
            cancellationToken).ConfigureAwait(false);

        var followMe = result.Data?.FetchFollowMe;
        return string.IsNullOrEmpty(followMe?.ExtensionId) ? null : followMe;
    }

    /// <inheritdoc />
    public async Task<MutationResult> UpdateAsync(UpdateFollowMeRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = await GraphQL.ExecuteMutationAsync<UpdateFollowMeResponse>(
            UpdateMutation,
            new { input = request },
            cancellationToken).ConfigureAwait(false);

        return ToResult(response.UpdateFollowMe);
    }

    /// <inheritdoc />
    public async Task<MutationResult> EnableAsync(string extensionId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(extensionId);

        var response = await GraphQL.ExecuteMutationAsync<EnableFollowMeResponse>(
            EnableMutation,
            new { input = new { extensionId } },
            cancellationToken).ConfigureAwait(false);

        return ToResult(response.EnableFollowMe);
    }

    /// <inheritdoc />
    public async Task<MutationResult> DisableAsync(string extensionId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(extensionId);

        var response = await GraphQL.ExecuteMutationAsync<DisableFollowMeResponse>(
            DisableMutation,
            new { input = new { extensionId } },
            cancellationToken).ConfigureAwait(false);

        return ToResult(response.DisableFollowMe);
    }

    private sealed class FollowMeResponse
    {
        public FollowMeDto? FetchFollowMe { get; set; }
    }

    private sealed class UpdateFollowMeResponse
    {
        public MutationPayload? UpdateFollowMe { get; set; }
    }

    private sealed class EnableFollowMeResponse
    {
        public MutationPayload? EnableFollowMe { get; set; }
    }

    private sealed class DisableFollowMeResponse
    {
        public MutationPayload? DisableFollowMe { get; set; }
    }
}
