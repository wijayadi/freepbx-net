namespace Sengsara.Freepbx.Abstractions.Models;

/// <summary>
/// Represents a FreePBX queue
/// </summary>
public class QueueDto
{
    public string Id { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
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
    public DateTime? CreateDate { get; set; }
    public DateTime? ModifyDate { get; set; }
    public bool Enabled { get; set; }
}

/// <summary>
/// Represents a member of a FreePBX queue
/// </summary>
public class QueueMemberDto
{
    public string Id { get; set; } = string.Empty;
    public string QueueId { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? Penalty { get; set; }
    public bool Paused { get; set; }
    public string? StateInterface { get; set; }
    public string? MemberName { get; set; }
    public string? Status { get; set; }
    public int? CallsTaken { get; set; }
    public TimeSpan? LastCall { get; set; }
    public TimeSpan? WrapUpTime { get; set; }
}