using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Sengsara.Freepbx.Abstractions.Interfaces;
using Sengsara.Freepbx.Abstractions.Interfaces.Services;
using Sengsara.Freepbx.Abstractions.Models;

namespace Sengsara.Freepbx.Services;

/// <summary>
/// Service for FreePBX queues. Queues are not exposed through GraphQL on
/// FreePBX 17, so this service uses the REST API.
/// </summary>
public sealed class QueueService : IQueueService
{
    private const string QueuesPath = "queues/";

    private readonly IFreepbxRestClient _rest;
    private readonly ILogger<QueueService> _logger;

    /// <summary>
    /// Creates a new queue service.
    /// </summary>
    public QueueService(IFreepbxRestClient rest, ILogger<QueueService> logger)
    {
        _rest = rest ?? throw new ArgumentNullException(nameof(rest));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<QueueDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var queues = await _rest.GetAsync<Dictionary<string, QueueDto>>(QueuesPath, cancellationToken).ConfigureAwait(false);
        if (queues is null)
        {
            return [];
        }

        foreach (var (key, value) in queues)
        {
            value.Extension = string.IsNullOrEmpty(value.Extension) ? key : value.Extension;
        }

        return queues.Values.OrderBy(q => q.Extension, StringComparer.Ordinal).ToList();
    }

    /// <inheritdoc />
    public async Task<QueueDto?> GetByIdAsync(string queueExtension, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queueExtension);

        return await _rest.GetAsync<QueueDto>($"{QueuesPath}{Uri.EscapeDataString(queueExtension)}", cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<QueueMembers> GetMembersAsync(string queueExtension, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queueExtension);

        var entry = await _rest
            .GetAsync<MemberEntry>($"{QueuesPath}members/{Uri.EscapeDataString(queueExtension)}", cancellationToken)
            .ConfigureAwait(false);

        return entry?.ToMembers() ?? QueueMembers.Empty();
    }

    /// <inheritdoc />
    public async Task<IReadOnlyDictionary<string, QueueMembers>> GetAllMembersAsync(CancellationToken cancellationToken = default)
    {
        var all = await _rest
            .GetAsync<Dictionary<string, MemberEntry>>($"{QueuesPath}members", cancellationToken)
            .ConfigureAwait(false);

        if (all is null)
        {
            return new Dictionary<string, QueueMembers>();
        }

        return all.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToMembers(), StringComparer.Ordinal);
    }

    /// <inheritdoc />
    public async Task<bool> SetMembersAsync(string queueExtension, QueueMembers members, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queueExtension);
        ArgumentNullException.ThrowIfNull(members);

        var body = new MemberUpdateBody
        {
            Member = string.Join('\n', members.Static),
            DynamicMembers = string.Join('\n', members.Dynamic)
        };

        var result = await _rest
            .PutAsync<bool>($"{QueuesPath}members/{Uri.EscapeDataString(queueExtension)}", body, cancellationToken)
            .ConfigureAwait(false);

        _logger.LogInformation("Updated members for queue {Queue}: {Result}", queueExtension, result);
        return result;
    }

    /// <inheritdoc />
    public async Task<bool> AddMemberAsync(string queueExtension, string member, bool dynamic = false, int penalty = 0, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(member);

        var members = await GetMembersAsync(queueExtension, cancellationToken).ConfigureAwait(false);
        var target = dynamic ? members.Dynamic : members.Static;
        var value = dynamic && penalty > 0 ? $"{member},{penalty}" : member;

        if (!target.Contains(value, StringComparer.Ordinal))
        {
            target.Add(value);
        }

        return await SetMembersAsync(queueExtension, members, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<bool> RemoveMemberAsync(string queueExtension, string member, bool dynamic = false, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(member);

        var members = await GetMembersAsync(queueExtension, cancellationToken).ConfigureAwait(false);
        var target = dynamic ? members.Dynamic : members.Static;

        target.RemoveAll(m =>
            string.Equals(m, member, StringComparison.Ordinal) ||
            m.StartsWith(member + ",", StringComparison.Ordinal));

        return await SetMembersAsync(queueExtension, members, cancellationToken).ConfigureAwait(false);
    }

    private sealed class MemberEntry
    {
        [JsonPropertyName("member")]
        public List<string>? Member { get; set; }

        [JsonPropertyName("members")]
        public List<string>? Members { get; set; }

        [JsonPropertyName("dynmembers")]
        public List<string>? DynamicMembers { get; set; }

        public QueueMembers ToMembers() => new()
        {
            Static = Member ?? Members ?? [],
            Dynamic = DynamicMembers ?? []
        };
    }

    private sealed class MemberUpdateBody
    {
        [JsonPropertyName("member")]
        public string Member { get; set; } = string.Empty;

        [JsonPropertyName("dynmembers")]
        public string DynamicMembers { get; set; } = string.Empty;
    }
}
