using Sengsara.Freepbx.Abstractions.Models;

namespace Sengsara.Freepbx.Abstractions.Interfaces.Services;

/// <summary>
/// Service for managing FreePBX extensions
/// </summary>
public interface IExtensionService
{
    /// <summary>
    /// Gets all extensions
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of extensions</returns>
    Task<IReadOnlyList<ExtensionDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an extension by ID
    /// </summary>
    /// <param name="extensionId">Extension ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Extension details</returns>
    Task<ExtensionDto?> GetByIdAsync(string extensionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new extension
    /// </summary>
    /// <param name="request">Create extension request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created extension</returns>
    Task<ExtensionDto> CreateAsync(CreateExtensionRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing extension
    /// </summary>
    /// <param name="extensionId">Extension ID</param>
    /// <param name="request">Update extension request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated extension</returns>
    Task<ExtensionDto> UpdateAsync(string extensionId, UpdateExtensionRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an extension
    /// </summary>
    /// <param name="extensionId">Extension ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if deletion was successful</returns>
    Task<bool> DeleteAsync(string extensionId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Request model for creating an extension
/// </summary>
public class CreateExtensionRequest
{
    public required string Extension { get; set; }
    public required string Name { get; set; }
    public string? Email { get; set; }
    public string? Department { get; set; }
    public string? Description { get; set; }
    public int? OutboundCid { get; set; }
}

/// <summary>
/// Request model for updating an extension
/// </summary>
public class UpdateExtensionRequest
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Department { get; set; }
    public string? Description { get; set; }
    public int? OutboundCid { get; set; }
}