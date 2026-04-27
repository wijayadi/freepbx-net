namespace Sengsara.Freepbx.Abstractions.Models;

/// <summary>
/// Represents a FreePBX agent (user/extension)
/// </summary>
public class AgentDto
{
    public string Id { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Department { get; set; }
    public string? Language { get; set; }
    public string? Timezone { get; set; }
    public bool Enabled { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? ModifyDate { get; set; }
}

/// <summary>
/// Request model for creating an agent
/// </summary>
public class CreateAgentRequest
{
    public required string Extension { get; set; }
    public required string Username { get; set; }
    public required string DisplayName { get; set; }
    public required string Password { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Department { get; set; }
    public string? Language { get; set; }
    public string? Timezone { get; set; }
}

/// <summary>
/// Request model for updating an agent
/// </summary>
public class UpdateAgentRequest
{
    public string? DisplayName { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Department { get; set; }
    public string? Language { get; set; }
    public string? Timezone { get; set; }
}