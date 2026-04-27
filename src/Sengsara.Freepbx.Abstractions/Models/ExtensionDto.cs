namespace Sengsara.Freepbx.Abstractions.Models;

/// <summary>
/// Represents a FreePBX extension
/// </summary>
public class ExtensionDto
{
    public string Id { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Department { get; set; }
    public string? Description { get; set; }
    public int? OutboundCid { get; set; }
    public string? DeviceType { get; set; }
    public string? UserLevel { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? ModifyDate { get; set; }
    public bool Enabled { get; set; }
}