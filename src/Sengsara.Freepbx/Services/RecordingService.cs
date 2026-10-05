using Microsoft.Extensions.Logging;
using Sengsara.Freepbx.Abstractions.Interfaces;
using Sengsara.Freepbx.Abstractions.Interfaces.Services;
using Sengsara.Freepbx.Abstractions.Models;

namespace Sengsara.Freepbx.Services;

/// <summary>
/// Service for managing FreePBX system recordings through GraphQL.
/// </summary>
public sealed class RecordingService : GraphQLServiceBase, IRecordingService
{
    private const string GetAllQuery = @"
        query GetRecordings {
          fetchAllRecordings {
            status
            message
            recordings { " + GraphQLFields.Recording + @" }
          }
        }";

    // NOTE: the fetchRecordingFiles(search:) resolver is broken on some FreePBX
    // 17 builds (it raises an internal server error), so files are always read
    // from fetchAllRecordings and filtered client side.
    private const string GetFilesQuery = @"
        query GetAllRecordingFiles {
          fetchAllRecordings {
            playbackFiles: recordings { playback }
          }
        }";

    private const string CreateMutation = @"
        mutation AddRecording($input: addRecordingInput!) {
          addRecording(input: $input) { status message id }
        }";

    private const string UpdateMutation = @"
        mutation UpdateRecording($input: updateRecordingInput!) {
          updateRecording(input: $input) { status message id }
        }";

    private const string DeleteMutation = @"
        mutation DeleteRecording($input: deleteRecordingInput!) {
          deleteRecording(input: $input) { status message id }
        }";

    /// <summary>
    /// Creates a new recording service.
    /// </summary>
    public RecordingService(IGraphQLExecutor graphQL, ILogger<RecordingService> logger)
        : base(graphQL, logger)
    {
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<RecordingDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await GraphQL.ExecuteQueryAsync<RecordingsResponse>(GetAllQuery, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.FetchAllRecordings?.Recordings ?? [];
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<string>> GetFilesAsync(string? search = null, CancellationToken cancellationToken = default)
    {
        var response = await GraphQL.ExecuteQueryAsync<RecordingFilesResponse>(GetFilesQuery, cancellationToken: cancellationToken).ConfigureAwait(false);

        var playback = (response.FetchAllRecordings?.PlaybackFiles ?? [])
            .SelectMany(recording => recording.Playback ?? []);

        return playback
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .Where(p => string.IsNullOrWhiteSpace(search) || p.Contains(search, StringComparison.OrdinalIgnoreCase))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(p => p, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    /// <inheritdoc />
    public async Task<MutationResult> SaveAsync(SaveRecordingRequest request, bool create, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (create)
        {
            var created = await GraphQL.ExecuteMutationAsync<RecordingPayloadResponse>(
                CreateMutation,
                new { input = request },
                cancellationToken).ConfigureAwait(false);

            return ToResult(created.AddRecording);
        }

        var updated = await GraphQL.ExecuteMutationAsync<RecordingPayloadResponse>(
            UpdateMutation,
            new { input = request },
            cancellationToken).ConfigureAwait(false);

        return ToResult(updated.UpdateRecording);
    }

    /// <inheritdoc />
    public async Task<MutationResult> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        var response = await GraphQL.ExecuteMutationAsync<RecordingPayloadResponse>(
            DeleteMutation,
            new { input = new { id } },
            cancellationToken).ConfigureAwait(false);

        return ToResult(response.DeleteRecording);
    }

    private sealed class RecordingsResponse
    {
        public RecordingConnection? FetchAllRecordings { get; set; }
    }

    private sealed class RecordingConnection
    {
        public bool? Status { get; set; }

        public string? Message { get; set; }

        public List<RecordingDto>? Recordings { get; set; }
    }

    private sealed class RecordingFilesResponse
    {
        public RecordingFilesConnection? FetchAllRecordings { get; set; }
    }

    private sealed class RecordingFilesConnection
    {
        public List<RecordingPlayback>? PlaybackFiles { get; set; }
    }

    private sealed class RecordingPlayback
    {
        public List<string>? Playback { get; set; }
    }

    private sealed class RecordingPayloadResponse
    {
        public RecordingPayload? AddRecording { get; set; }

        public RecordingPayload? UpdateRecording { get; set; }

        public RecordingPayload? DeleteRecording { get; set; }
    }

    private sealed class RecordingPayload : MutationPayload
    {
        public string? Id { get; set; }
    }
}
