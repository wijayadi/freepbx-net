using Microsoft.Extensions.Logging;
using Sengsara.Freepbx.Abstractions.Interfaces;
using Sengsara.Freepbx.Abstractions.Interfaces.Services;
using Sengsara.Freepbx.Abstractions.Models;

namespace Sengsara.Freepbx.Services;

/// <summary>
/// Service for managing FreePBX inbound routes (DIDs) through GraphQL.
/// </summary>
public sealed class InboundRouteService : GraphQLServiceBase, IInboundRouteService
{
    private const string GetAllQuery = @"
        query GetInboundRoutes {
          allInboundRoutes {
            totalCount
            inboundRoutes { " + GraphQLFields.InboundRoute + @" }
          }
        }";

    private const string GetByIdQuery = @"
        query GetInboundRoute($id: ID!) {
          inboundRoute(id: $id) { " + GraphQLFields.InboundRoute + @" }
        }";

    private const string CreateMutation = @"
        mutation AddInboundRoute($input: addInboundRouteInput!) {
          addInboundRoute(input: $input) {
            status
            message
            inboundRoute { " + GraphQLFields.InboundRoute + @" }
          }
        }";

    private const string UpdateMutation = @"
        mutation UpdateInboundRoute($input: updateInboundRouteInput!) {
          updateInboundRoute(input: $input) {
            status
            message
            inboundRoute { " + GraphQLFields.InboundRoute + @" }
          }
        }";

    private const string DeleteMutation = @"
        mutation RemoveInboundRoute($input: removeInboundRouteInput!) {
          removeInboundRoute(input: $input) { status message deletedId }
        }";

    /// <summary>
    /// Creates a new inbound route service.
    /// </summary>
    public InboundRouteService(IGraphQLExecutor graphQL, ILogger<InboundRouteService> logger)
        : base(graphQL, logger)
    {
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<InboundRouteDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await GraphQL.ExecuteQueryAsync<InboundRoutesResponse>(GetAllQuery, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.AllInboundRoutes?.InboundRoutes ?? [];
    }

    /// <inheritdoc />
    public async Task<InboundRouteDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        var result = await GraphQL.TryExecuteQueryAsync<InboundRouteResponse>(
            GetByIdQuery,
            new { id },
            cancellationToken).ConfigureAwait(false);

        var route = result.Data?.InboundRoute;
        return string.IsNullOrEmpty(route?.Extension) ? null : route;
    }

    /// <inheritdoc />
    public async Task<InboundRouteDto?> CreateAsync(AddInboundRouteRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = await GraphQL.ExecuteMutationAsync<InboundRoutePayloadResponse>(
            CreateMutation,
            new { input = request },
            cancellationToken).ConfigureAwait(false);

        return response.AddInboundRoute?.InboundRoute;
    }

    /// <inheritdoc />
    public async Task<InboundRouteDto?> UpdateAsync(UpdateInboundRouteRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = await GraphQL.ExecuteMutationAsync<InboundRoutePayloadResponse>(
            UpdateMutation,
            new { input = request },
            cancellationToken).ConfigureAwait(false);

        return response.UpdateInboundRoute?.InboundRoute;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        var response = await GraphQL.ExecuteMutationAsync<RemoveInboundRouteResponse>(
            DeleteMutation,
            new { input = new { id } },
            cancellationToken).ConfigureAwait(false);

        return response.RemoveInboundRoute?.Status ?? false;
    }

    private sealed class InboundRoutesResponse
    {
        public InboundRouteConnection? AllInboundRoutes { get; set; }
    }

    private sealed class InboundRouteConnection
    {
        public int? TotalCount { get; set; }

        public List<InboundRouteDto>? InboundRoutes { get; set; }
    }

    private sealed class InboundRouteResponse
    {
        public InboundRouteDto? InboundRoute { get; set; }
    }

    private sealed class InboundRoutePayloadResponse
    {
        public InboundRoutePayload? AddInboundRoute { get; set; }

        public InboundRoutePayload? UpdateInboundRoute { get; set; }
    }

    private sealed class InboundRoutePayload : MutationPayload
    {
        public InboundRouteDto? InboundRoute { get; set; }
    }

    private sealed class RemoveInboundRouteResponse
    {
        public RemoveInboundRoutePayload? RemoveInboundRoute { get; set; }
    }

    private sealed class RemoveInboundRoutePayload : MutationPayload
    {
        public string? DeletedId { get; set; }
    }
}
