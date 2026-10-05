using Microsoft.Extensions.Logging;
using Sengsara.Freepbx.Abstractions.Interfaces;
using Sengsara.Freepbx.Abstractions.Models;

namespace Sengsara.Freepbx.Services;

/// <summary>
/// Standard FreePBX mutation payload containing a status flag and a message.
/// </summary>
internal class MutationPayload
{
    public bool? Status { get; set; }

    public string? Message { get; set; }
}

/// <summary>
/// Base class for services that talk to FreePBX over GraphQL.
/// </summary>
public abstract class GraphQLServiceBase
{
    /// <summary>GraphQL executor.</summary>
    protected IGraphQLExecutor GraphQL { get; }

    /// <summary>Logger.</summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// Creates a new service.
    /// </summary>
    protected GraphQLServiceBase(IGraphQLExecutor graphQL, ILogger logger)
    {
        GraphQL = graphQL ?? throw new ArgumentNullException(nameof(graphQL));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>Converts a mutation payload into a <see cref="MutationResult"/>.</summary>
    private protected static MutationResult ToResult(MutationPayload? payload)
        => new()
        {
            Success = payload?.Status ?? false,
            Message = payload?.Message
        };
}
