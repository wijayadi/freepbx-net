using Microsoft.Extensions.Logging;
using Sengsara.Freepbx.Abstractions.Interfaces;
using Sengsara.Freepbx.Abstractions.Interfaces.Services;
using Sengsara.Freepbx.Abstractions.Models;

namespace Sengsara.Freepbx.Services;

/// <summary>
/// Service for querying FreePBX call detail records through GraphQL.
/// </summary>
public sealed class CdrService : GraphQLServiceBase, ICdrService
{
    private const string GetQuery = @"
        query GetCdrs($first: Int, $startDate: String, $endDate: String, $orderby: cdrOrderBy) {
          fetchAllCdrs(first: $first, startDate: $startDate, endDate: $endDate, orderby: $orderby) {
            totalCount
            cdrs { " + GraphQLFields.Cdr + @" }
          }
        }";

    private const string GetByIdQuery = @"
        query GetCdr($id: ID) {
          fetchCdr(id: $id) { " + GraphQLFields.Cdr + @" }
        }";

    /// <summary>
    /// Creates a new CDR service.
    /// </summary>
    public CdrService(IGraphQLExecutor graphQL, ILogger<CdrService> logger)
        : base(graphQL, logger)
    {
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<CdrDto>> GetAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        int? first = null,
        CdrOrderBy orderBy = CdrOrderBy.Date,
        CancellationToken cancellationToken = default)
    {
        var start = startDate ?? DateTime.UtcNow.AddDays(-7);
        var end = endDate ?? DateTime.UtcNow;

        var response = await GraphQL.ExecuteQueryAsync<CdrsResponse>(
            GetQuery,
            new
            {
                first = first ?? 100,
                startDate = start.ToString("yyyy-MM-dd"),
                endDate = end.ToString("yyyy-MM-dd"),
                orderby = orderBy == CdrOrderBy.Duration ? "duration" : "date"
            },
            cancellationToken).ConfigureAwait(false);

        return response.FetchAllCdrs?.Cdrs ?? [];
    }

    /// <inheritdoc />
    public async Task<CdrDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        var result = await GraphQL.TryExecuteQueryAsync<CdrResponse>(
            GetByIdQuery,
            new { id },
            cancellationToken).ConfigureAwait(false);

        var cdr = result.Data?.FetchCdr;
        return string.IsNullOrEmpty(cdr?.UniqueId) ? null : cdr;
    }

    private sealed class CdrsResponse
    {
        public CdrConnection? FetchAllCdrs { get; set; }
    }

    private sealed class CdrConnection
    {
        public int? TotalCount { get; set; }

        public List<CdrDto>? Cdrs { get; set; }
    }

    private sealed class CdrResponse
    {
        public CdrDto? FetchCdr { get; set; }
    }
}
