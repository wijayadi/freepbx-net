using Microsoft.Extensions.Logging;
using Sengsara.Freepbx.Abstractions.Interfaces;
using Sengsara.Freepbx.Abstractions.Interfaces.Services;
using Sengsara.Freepbx.Abstractions.Models;

namespace Sengsara.Freepbx.Services;

/// <summary>
/// Service for managing FreePBX Core extensions through GraphQL.
/// </summary>
public sealed class ExtensionService : GraphQLServiceBase, IExtensionService
{
    private const string GetAllQuery = @"
        query GetExtensions {
          fetchAllExtensions {
            totalCount
            count
            extension { " + GraphQLFields.Extension + @" }
          }
        }";

    private const string GetAllValidQuery = @"
        query GetValidExtensions {
          fetchAllValidExtensions {
            totalCount
            count
            extension { " + GraphQLFields.Extension + @" }
          }
        }";

    private const string GetByIdQuery = @"
        query GetExtension($extensionId: ID) {
          fetchExtension(extensionId: $extensionId) { " + GraphQLFields.Extension + @" }
        }";

    private const string CreateMutation = @"
        mutation AddExtension($input: addExtensionInput!) {
          addExtension(input: $input) { status message }
        }";

    private const string UpdateMutation = @"
        mutation UpdateExtension($input: updateExtensionInput!) {
          updateExtension(input: $input) { status message }
        }";

    private const string DeleteMutation = @"
        mutation DeleteExtension($input: deleteExtensionInput!) {
          deleteExtension(input: $input) { status message }
        }";

    private const string CreateRangeMutation = @"
        mutation CreateRangeOfExtensions($input: CreateRangeofExtensionInput!) {
          createRangeofExtension(input: $input) { status message }
        }";

    /// <summary>
    /// Creates a new extension service.
    /// </summary>
    public ExtensionService(IGraphQLExecutor graphQL, ILogger<ExtensionService> logger)
        : base(graphQL, logger)
    {
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<ExtensionDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await GraphQL.ExecuteQueryAsync<ExtensionsResponse>(GetAllQuery, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.FetchAllExtensions?.Extension ?? [];
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<ExtensionDto>> GetAllValidAsync(CancellationToken cancellationToken = default)
    {
        var response = await GraphQL.ExecuteQueryAsync<ExtensionsResponse>(GetAllValidQuery, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.FetchAllValidExtensions?.Extension ?? [];
    }

    /// <inheritdoc />
    public async Task<ExtensionDto?> GetByIdAsync(string extensionId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(extensionId);

        var result = await GraphQL.TryExecuteQueryAsync<ExtensionResponse>(
            GetByIdQuery,
            new { extensionId },
            cancellationToken).ConfigureAwait(false);

        var extension = result.Data?.FetchExtension;
        return string.IsNullOrEmpty(extension?.ExtensionId) ? null : extension;
    }

    /// <inheritdoc />
    public async Task<MutationResult> CreateAsync(AddExtensionRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = await GraphQL.ExecuteMutationAsync<AddExtensionResponse>(
            CreateMutation,
            new { input = request },
            cancellationToken).ConfigureAwait(false);

        Logger.LogInformation("Created extension {ExtensionId}: {Success}", request.ExtensionId, response.AddExtension?.Status);
        return ToResult(response.AddExtension);
    }

    /// <inheritdoc />
    public async Task<MutationResult> UpdateAsync(UpdateExtensionRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = await GraphQL.ExecuteMutationAsync<UpdateExtensionResponse>(
            UpdateMutation,
            new { input = request },
            cancellationToken).ConfigureAwait(false);

        Logger.LogInformation("Updated extension {ExtensionId}: {Success}", request.ExtensionId, response.UpdateExtension?.Status);
        return ToResult(response.UpdateExtension);
    }

    /// <inheritdoc />
    public async Task<MutationResult> DeleteAsync(string extensionId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(extensionId);

        var response = await GraphQL.ExecuteMutationAsync<DeleteExtensionResponse>(
            DeleteMutation,
            new { input = new { extensionId } },
            cancellationToken).ConfigureAwait(false);

        Logger.LogInformation("Deleted extension {ExtensionId}: {Success}", extensionId, response.DeleteExtension?.Status);
        return ToResult(response.DeleteExtension);
    }

    /// <inheritdoc />
    public async Task<MutationResult> CreateRangeAsync(CreateExtensionRangeRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = await GraphQL.ExecuteMutationAsync<CreateRangeResponse>(
            CreateRangeMutation,
            new { input = request },
            cancellationToken).ConfigureAwait(false);

        return ToResult(response.CreateRangeOfExtension);
    }

    private sealed class ExtensionsResponse
    {
        public ExtensionConnection? FetchAllExtensions { get; set; }

        public ExtensionConnection? FetchAllValidExtensions { get; set; }
    }

    private sealed class ExtensionConnection
    {
        public int? TotalCount { get; set; }

        public int? Count { get; set; }

        public List<ExtensionDto>? Extension { get; set; }
    }

    private sealed class ExtensionResponse
    {
        public ExtensionDto? FetchExtension { get; set; }
    }

    private sealed class AddExtensionResponse
    {
        public MutationPayload? AddExtension { get; set; }
    }

    private sealed class UpdateExtensionResponse
    {
        public MutationPayload? UpdateExtension { get; set; }
    }

    private sealed class DeleteExtensionResponse
    {
        public MutationPayload? DeleteExtension { get; set; }
    }

    private sealed class CreateRangeResponse
    {
        public MutationPayload? CreateRangeOfExtension { get; set; }
    }
}
